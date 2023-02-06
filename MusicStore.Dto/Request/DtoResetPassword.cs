namespace MusicStore.Dto.Request;

public record DtoResetPassword(string Token, string Email, string NewPassword);