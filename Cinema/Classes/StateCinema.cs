using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class StateCinema
    {
        public int IdPost { get; set; }
        public int? CountStaffers { get; set; }

        public virtual Post IdPostNavigation { get; set; }
    }
}
