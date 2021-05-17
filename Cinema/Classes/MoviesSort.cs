using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class MoviesSort
    {
        public int IdFilm { get; set; }
        public int IdMovie { get; set; }
        public DateTime MovieTime { get; set; }
        public string NameFilm { get; set; }
        public int? IdRoom { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public int? SalaryForPlace { get; set; }
        public string TypeView { get; set; }
    }
}
