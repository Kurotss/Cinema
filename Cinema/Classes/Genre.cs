using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Genre
    {
        public int IdFilm { get; set; }
        public string Genre1 { get; set; }

        public virtual Film IdFilmNavigation { get; set; }
    }
}
