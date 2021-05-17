using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Place
    {
        public int IdRoom { get; set; }
        public int NumberRow { get; set; }
        public int NumberPlace { get; set; }

        public virtual Room IdRoomNavigation { get; set; }
    }
}
