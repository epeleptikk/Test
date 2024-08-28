using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp7
{
    public class Passenger
    {
        [Key]
        public int PassengerID { get; set; }
        public bool HasTicket { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
