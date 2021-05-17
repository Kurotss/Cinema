using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class User
    {
        public string Email { get; set; }
        public string PasswordUser { get; set; }
        public int? IdRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual Client Client { get; set; }
    }
}
