using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class AvailablePlace
    {
        public int IdMovie { get; set; }
        public int? IdTicket { get; set; }
        public int NumberRow { get; set; }
        public int NumberPlace { get; set; }
    }
}
