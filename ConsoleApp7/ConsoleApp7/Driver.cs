using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp7
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int VehicleID { get; set; }

        [ForeignKey("VehicleID")]
        public Vehicle Vehicle { get; set; }

    }
}
