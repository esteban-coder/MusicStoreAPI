namespace MusicStore.Entities.Infos;

public class SaleInfo
{
    public int SaleId { get; set; }
    public DateTime DateEvent { get; set; }

    public string Genre { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string Title { get; set; } = default!;
    public string OperationNumber { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? Email { get; set; }
    public int Quantity { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Total { get; set; }

}