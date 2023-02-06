using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStore.Entities;

public class Sale : EntityBase
{
    [ForeignKey(nameof(CustomerForeignKey))]
    public Customer Customer { get; set; } = default!;

    public int CustomerForeignKey { get; set; } = default!;

    public DateTime SaleDate { get; set; }

    public string OperationNumber { get; set; } = default!;

    public decimal Total { get; set; }

    public Concert Concert { get; set; } = default!;

    public int ConcertId { get; set; }

    public int Quantity { get; set; }

}