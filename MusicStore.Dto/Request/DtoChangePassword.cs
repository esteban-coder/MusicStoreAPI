namespace MusicStore.Dto.Request;

public record DtoChangePassword(string Email, string OldPassword, string NewPassword);