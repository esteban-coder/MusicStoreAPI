using System;
using System.Collections.Generic;

namespace MusicStore.DatabaseFirst
{
    public partial class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public bool Status { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
