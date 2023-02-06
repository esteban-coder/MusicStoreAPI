using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponseGeneric<string>> RegisterAsync(RegisterUserDtoRequest request);

    Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request);

    Task<BaseResponse> RequestTokenToResetPasswordAsync(DtoRequestPassword request);

    Task<BaseResponse> ResetPasswordAsync(DtoResetPassword request);

    Task<BaseResponse> ChangePasswordAsync(DtoChangePassword request);
}