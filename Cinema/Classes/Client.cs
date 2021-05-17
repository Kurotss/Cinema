using System;
using System.Collections.Generic;

#nullable disable

namespace Cinema
{
    public partial class Client
    {
        public Client()
        {
            Tickets = new HashSet<Ticket>();
        }

        public string Email { get; set; }
        public string NumberTelephon { get; set; }
        public string Surname { get; set; }
        public string NameClient { get; set; }

        public virtual User EmailNavigation { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
