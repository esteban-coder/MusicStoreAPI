using System;
using System.Collections.Generic;

namespace MusicStore.DatabaseFirst
{
    public partial class Genre
    {
        public Genre()
        {
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool Status { get; set; }
        public string? Comments { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
