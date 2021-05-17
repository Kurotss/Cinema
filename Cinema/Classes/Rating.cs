using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Rating
    {
        public int IdFilm { get; set; }
        public double Rating1 { get; set; }
        public int CountOfPeopleInRaiting { get; set; }

        public virtual Film IdFilmNavigation { get; set; }
    }
}
