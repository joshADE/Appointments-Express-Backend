using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class Permission
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string name { get; set; }
    }
}
