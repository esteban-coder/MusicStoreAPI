namespace MusicStore.Dto.Request;

public class LoginDtoRequest
{
    public string User { get; set; } = default!;
    public string Password { get; set; } = default!;
}