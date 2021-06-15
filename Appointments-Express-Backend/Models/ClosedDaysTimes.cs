using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class ClosedDaysTimes
    {
        public enum RepeatInterval
        {
            everyDayOfWeek,
            currentDayOfWeek,
            currentDayOfMonth,
            currentDayOfYear
        }

        [Key]
        public int id { get; set; }

        public int storeId { get; set; }

        [Required]
        public DateTime from { get; set; }

        [Required]
        public DateTime to { get; set; }

        [Required]
        public bool repeat { get; set; }

        public RepeatInterval? repeatInterval { get; set; }

        [ForeignKey("storeId")]
        public virtual Store store { get; set; }
    }
}
