using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class FilmsRaitingsGenre
    {
        public int IdFilm { get; set; }
        public string NameFilm { get; set; }
        public byte[] Poster { get; set; }
        public string Genre { get; set; }
        public int? AgeLimit { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
    }
}
