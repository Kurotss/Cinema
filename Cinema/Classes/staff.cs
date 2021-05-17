using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class staff
    {
        public int IdStaffer { get; set; }
        public string Surname { get; set; }
        public string NameStaffer { get; set; }
        public string NumberTelephon { get; set; }
        public string Email { get; set; }
        public int IdPost { get; set; }

        public virtual Post IdPostNavigation { get; set; }
    }
}
