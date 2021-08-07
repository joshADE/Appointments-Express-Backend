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
        public string firstName { get; set; }

        [Required]
        [MinLength(2)]
        public string lastName { get; set; }

        [Required]
        [MinLength(2)]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string email { get; set; }

        public string? avatarPublicId { get; set; }
        public string? avatarUrl { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdAt { get; set; }


    }
}
