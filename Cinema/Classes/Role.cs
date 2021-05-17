using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int IdRole { get; set; }
        public string NameRole { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
