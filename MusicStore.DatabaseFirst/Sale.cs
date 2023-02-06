using System;
using System.Collections.Generic;

namespace MusicStore.DatabaseFirst
{
    public partial class Sale
    {
        public int Id { get; set; }
        public int CustomerForeignKey { get; set; }
        public DateTime SaleDate { get; set; }
        public string OperationNumber { get; set; } = null!;
        public decimal Total { get; set; }
        public int ConcertId { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }

        public virtual Event Concert { get; set; } = null!;
        public virtual Customer CustomerForeignKeyNavigation { get; set; } = null!;
    }
}
