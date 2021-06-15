using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(2)]
        [Column(TypeName ="nvarchar(100)")]
        public string firstName { get; set; }

        [Required]
        [MinLength(2)]
        [Column(TypeName = "nvarchar(100)")]
        public string lastName { get; set; }

        [Required]
        [MinLength(2)]
        [Column(TypeName = "nvarchar(100)")]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string email { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime createdAt { get; set; }


    }
}
