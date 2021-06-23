using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class Store
    {

        public Store()
        {
            isQuickProfile = false;
        }

        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(2)]
        public string name { get; set; }

        [Required]
        [MinLength(2)]
        public string location { get; set; }

        [Required]
        public int minTimeBlock { get; set; }

        [Required]
        public int maxTimeBlock { get; set; }

        public bool isQuickProfile { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdAt { get; set; }
    }
}
