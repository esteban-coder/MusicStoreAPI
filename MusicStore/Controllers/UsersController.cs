using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Services.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoginDtoResponse), 200)]
    public async Task<IActionResult> Login([FromBody] LoginDtoRequest request)
    {
        return Ok(await _service.LoginAsync(request));
    }

    [HttpPost("Register")]
    [ProducesResponseType(typeof(BaseResponseGeneric<string>), 200)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDtoRequest request)
    {
        return Ok(await _service.RegisterAsync(request));
    }

    [HttpPost("SendTokenToResetPassword")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    public async Task<IActionResult> SendTokenToResetPassword([FromBody] DtoRequestPassword request)
    {
        return Ok(await _service.RequestTokenToResetPasswordAsync(request));
    }

    [HttpPost("ResetPassword")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    public async Task<IActionResult> ResetPassword([FromBody] DtoResetPassword request)
    {
        return Ok(await _service.ResetPasswordAsync(request));
    }

    [HttpPost("ChangePassword")]
    [ProducesResponseType(typeof(BaseResponse), 200)]
    public async Task<IActionResult> ChangePassword([FromBody] DtoChangePassword request)
    {
        return Ok(await _service.ChangePasswordAsync(request));
    }
}