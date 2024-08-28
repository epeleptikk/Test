using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp7
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public int NumberOfSeats { get; set; }

        public Driver Driver { get; set; }
        public ICollection<Route> Routes { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
