using System.ComponentModel.DataAnnotations;

namespace MusicStore.Dto.Request;

public class RegisterUserDtoRequest
{
    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string DocumentNumber { get; set; } = default!;

    [Required]
    public string DocumentType { get; set; } = default!;
    
    public int Age { get; set; }

    [Required]
    public string Password { get; set; } = default!;

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = default!;
}