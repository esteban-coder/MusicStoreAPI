using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicStore.Dto.Request;

public class ConcertDtoRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    public int IdGenre { get; set; }

    [Required]
    // format date yyyy-mm-dd
    public string DateEvent { get; set; } = null!;

    [Required]
    // Regular expression for time HH:MM:ss
    public string TimeEvent { get; set; } = null!;

    // Esto hara que se ignore el campo en el JSON  [JsonIgnore] 
    public string Place { get; set; } = default!;

    public string? Base64Image { get; set; }

    public string? FileName { get; set; }

    public decimal UnitPrice { get; set; }

    [Range(1, 9999, ErrorMessage = "El valor debe estar entre 1 y 9999")]
    public int TicketsQuantity { get; set; }
}