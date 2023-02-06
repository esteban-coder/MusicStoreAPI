using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MusicStore.Repositories;

namespace MusicStore.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<MusicStoreUserIdentity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<AppSettings> _options;
    private readonly ILogger<UserService> _logger;
    private readonly IEmailSender _emailSender;
    private readonly ICustomerRepository _customerRepository;

    public UserService(UserManager<MusicStoreUserIdentity> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IOptions<AppSettings> options, 
        ILogger<UserService> logger, 
        IEmailSender emailSender,
        ICustomerRepository customerRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _options = options;
        _logger = logger;
        _emailSender = emailSender;
        _customerRepository = customerRepository;
    }

    public async Task<BaseResponseGeneric<string>> RegisterAsync(RegisterUserDtoRequest request)
    {
        var response = new BaseResponseGeneric<string>();
        try
        {
            var user = new MusicStoreUserIdentity
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                DocumentNumber = request.DocumentNumber,
                DocumentType = request.DocumentType,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                var userIdentity = await _userManager.FindByEmailAsync(request.Email);

                if (userIdentity != null)
                {
                    if (!await _roleManager.RoleExistsAsync(Constants.RoleAdmin))
                        await _roleManager.CreateAsync(new IdentityRole(Constants.RoleAdmin));

                    if (!await _roleManager.RoleExistsAsync(Constants.RoleCustomer))
                        await _roleManager.CreateAsync(new IdentityRole(Constants.RoleCustomer));

                    if (await _userManager.Users.CountAsync() == 1)
                        await _userManager.AddToRoleAsync(userIdentity, Constants.RoleAdmin);
                    else
                        await _userManager.AddToRoleAsync(userIdentity, Constants.RoleCustomer);

                    // Creamos el objeto Customer con el registro del usuario.
                    var customer = new Customer
                    {
                        FullName = $"{request.FirstName} {request.LastName}",
                        Email = request.Email
                    };

                    // Guardamos el registro del Customer.
                    await _customerRepository.AddAsync(customer);

                    // Enviar un correo

                    await _emailSender.SendEmailAsync(new EmailMessageInfo(user.Email, $"{userIdentity.FirstName} {userIdentity.LastName}", "Registro de usuario", $"Se creo correctamente el usuario {userIdentity.Email} en nuestro sistema"));
                }

                response.Success = true;
                response.Data = user.Id;
            }
            else
            {
                response.Success = false;
                var sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Length = 0;
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "An error occurred while creating the user";
            _logger.LogError(ex, "Error al registrar {Message}", ex.Message);
        }

        return response;
    }

    public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
    {
        var response = new LoginDtoResponse();

        try
        {
            var identity = await _userManager.FindByEmailAsync(request.User);

            if (identity == null)
            {
                throw new ApplicationException("Usuario no existe");
            }

            var result = await _userManager.CheckPasswordAsync(identity, request.Password);
            if (!result)
                throw new ApplicationException("Clave incorrecta");

            response.FullName = identity.FirstName + " " + identity.LastName;

            var expiredDate = DateTime.Now.AddDays(1);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{identity.FirstName} {identity.LastName}"),
                new Claim(ClaimTypes.Email, identity.Email),
                new Claim(ClaimTypes.Expiration, expiredDate.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var roles = await _userManager.GetRolesAsync(identity);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            response.Roles = new List<string>();
            response.Roles.AddRange(roles);

            // Creacion de token JWT
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.Secret));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(_options.Value.Jwt.Issuer, _options.Value.Jwt.Audience,
                claims, DateTime.Now, expiredDate);

            var token = new JwtSecurityToken(header, payload);

            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            response.Success = true;

            _logger.LogInformation("Usuario {User} logueado correctamente", identity.Email);
        }
        catch (Exception ex)
        {
            response.ErrorMessage = ex.Message;
            response.Success = false;
            _logger.LogError(ex, "Error al iniciar sesión {Message}", ex.Message);
        }


        return await Task.FromResult(response);
    }

    public async Task<BaseResponse> RequestTokenToResetPasswordAsync(DtoRequestPassword request)
    {
        var response = new BaseResponse();

        try
        {
            var userIdentity = await _userManager.FindByEmailAsync(request.Email);

            if (userIdentity == null)
            {
                throw new ApplicationException("Usuario no existe");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(userIdentity);

            // Enviar un correo
            await _emailSender.SendEmailAsync(new EmailMessageInfo(userIdentity.Email, $"{userIdentity.FirstName} {userIdentity.LastName}", "Restablecer contraseña", $"Para restablecer su contraseña copie el siguiente token: {token}"));
            
            _logger.LogInformation("Se envio correo con solicitud para reseteo");

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "No se pudo completar la solicitud";
            _logger.LogError(ex, "Error al resetear password {Message}", ex.Message);
        }
        
        return response;
    }

    public async Task<BaseResponse> ResetPasswordAsync(DtoResetPassword request)
    {
        var response = new BaseResponse();

        try
        {
            var userIdentity = await _userManager.FindByEmailAsync(request.Email);

            if (userIdentity == null)
            {
                throw new ApplicationException("Usuario no existe");
            }

            var result = await _userManager.ResetPasswordAsync(userIdentity, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Length = 0;
            }
            else
            {
                response.Success = true;
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "No se pudo completar la solicitud";
            _logger.LogError(ex, "Error al resetear password {Message}", ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse> ChangePasswordAsync(DtoChangePassword request)
    {
        var response = new BaseResponse();

        try
        {
            var userIdentity = await _userManager.FindByEmailAsync(request.Email);

            if (userIdentity == null)
            {
                throw new ApplicationException("Usuario no existe");
            }

            var result = await _userManager.ChangePasswordAsync(userIdentity, request.OldPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Length = 0;
            }
            else
            {
                response.Success = true;
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "No se pudo completar la solicitud";
            _logger.LogError(ex, "Error al cambiar password {Message}", ex.Message);
        }

        return response;
    }
}