using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Movie
    {
        public Movie()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int IdMovie { get; set; }
        public int IdFilm { get; set; }
        public DateTime MovieTime { get; set; }
        public int? IdRoom { get; set; }

        public virtual Film IdFilmNavigation { get; set; }
        public virtual Room IdRoomNavigation { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
