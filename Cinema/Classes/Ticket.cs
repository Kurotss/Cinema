using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Ticket
    {
        public int IdTicket { get; set; }
        public string Email { get; set; }
        public int IdMovie { get; set; }
        public int NumberPlace { get; set; }
        public int NumberRow { get; set; }
        public byte[] ImageTicket { get; set; }

        public virtual Client EmailNavigation { get; set; }
        public virtual Movie IdMovieNavigation { get; set; }
    }
}
