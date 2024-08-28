using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp7
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public int PassengerID { get; set; }
        public int VehicleID { get; set; }

        [ForeignKey("PassengerID")]
        public Passenger Passenger { get; set; }
        [ForeignKey("VehicleID")]
        public Vehicle Vehicle { get; set; }
    }
}
