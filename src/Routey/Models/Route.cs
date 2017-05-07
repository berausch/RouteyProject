using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Routey.Models
{
    [Table("Routes")]
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        public string Name { get; set; }

        public bool SaveRoute { get; set; }

        public List<Location> Locations { get; set; }

        public Route()
        {
        }
    }
}
