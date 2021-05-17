using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Form
    {
        public int IdForm { get; set; }
        public string Surname { get; set; }
        public string NameClient { get; set; }
        public string Telephon { get; set; }
        public int? Age { get; set; }
        public int? IdPost { get; set; }
        public string Email { get; set; }

        public virtual Post IdPostNavigation { get; set; }
    }
}
