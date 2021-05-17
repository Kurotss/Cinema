using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Post
    {
        public Post()
        {
            staff = new HashSet<staff>();
            Forms = new HashSet<Form>();
        }

        public int IdPost { get; set; }
        public string NamePost { get; set; }

        public virtual StateCinema StateCinema { get; set; }
        public virtual ICollection<staff> staff { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
    }
}
