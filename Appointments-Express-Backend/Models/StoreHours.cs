using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class StoreHours
    {
        public int storeId { get; set; }

        public int dayOfWeek { get; set; }

        [Required]
        public TimeSpan open { get; set; }
        
        [Required]
        public TimeSpan close { get; set; }

        [ForeignKey("storeId")]
        public virtual Store store { get; set; }
    }
}
