using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp7
{
    public class Route
    {
        [Key]
        public int RouteID { get; set; }
        public string RouteName { get; set; }
        public int VehicleID { get; set; }

        [ForeignKey("VehicleID")]
        public Vehicle Vehicle { get; set; }
    }
}
