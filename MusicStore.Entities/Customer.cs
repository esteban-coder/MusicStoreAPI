using System.ComponentModel.DataAnnotations;

namespace MusicStore.Entities;

public class Customer : EntityBase
{
    [StringLength(500)]
    public string Email { get; set; } = default!;

    [StringLength(200)]
    public string FullName { get; set; } = default!;
}