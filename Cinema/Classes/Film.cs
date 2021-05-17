using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Film
    {
        public Film()
        {
            Genres = new HashSet<Genre>();
            Movies = new HashSet<Movie>();
        }

        public int IdFilm { get; set; }
        public string NameFilm { get; set; }
        public int? AgeLimit { get; set; }
        public DateTime? AirDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte[] Poster { get; set; }
        public string Description { get; set; }

        public virtual Rating Rating { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
