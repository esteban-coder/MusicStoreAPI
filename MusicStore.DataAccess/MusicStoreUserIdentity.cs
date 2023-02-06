using Microsoft.AspNetCore.Identity;

namespace MusicStore.DataAccess;

public class MusicStoreUserIdentity : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    public string DocumentType { get; set; } = default!;

    public string DocumentNumber { get; set; } = default!;
}