using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStore.Entities;

[Table("Events")]
public class Concert : EntityBase
{
    //[ForeignKey(nameof(CodigoGenero))]
    public Genre Genre { get; set; } = default!;

    public int GenreId { get; set; }


    //public int CodigoGenero { get; set; }

    [StringLength(150)]
    public string Title { get; set; } = default!;

    [StringLength(500)]
    public string Description { get; set; } = default!;
    public DateTime DateEvent { get; set; }

    public string? ImageUrl { get; set; }

    [StringLength(100)]
    public string? Place { get; set; }
    public int TicketsQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool Finalized { get; set; }
}