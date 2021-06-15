using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class Role
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public string description { get; set; }
    }
}
