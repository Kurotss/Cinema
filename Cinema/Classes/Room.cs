using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Room
    {
        public Room()
        {
            Movies = new HashSet<Movie>();
            Places = new HashSet<Place>();
        }

        public int IdRoom { get; set; }
        public int? IdTypeView { get; set; }
        public int? CountPlaces { get; set; }
        public int? SalaryForPlace { get; set; }

        public virtual TypesView IdTypeViewNavigation { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Place> Places { get; set; }
    }
}
