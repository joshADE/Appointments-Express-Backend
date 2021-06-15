using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class Customer
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(2)]
        public string firstName { get; set; }

        [Required]
        [MinLength(2)]
        public string lastName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string email { get; set; }

    }
}
