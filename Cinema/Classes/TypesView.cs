using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class TypesView
    {
        public TypesView()
        {
            Rooms = new HashSet<Room>();
        }

        public int IdTypeView { get; set; }
        public string TypeView { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
