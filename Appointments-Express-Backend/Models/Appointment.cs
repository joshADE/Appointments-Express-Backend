using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Models
{
    public class Appointment
    {

        public enum AppointmentStatus
        {
            pending,
            complete,
            declined,
            accepted
        }

        [Key]
        public int id { get; set; }

        public int storeId { get; set; }
        public int customerId { get; set; }

        [Required]
        public DateTime start { get; set; }

        [Required]
        public DateTime end { get; set; }

        [Required]
        [MinLength(2)]
        public string title { get; set; }

        public string description { get; set; }

        public AppointmentStatus status { get; set; }



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdAt { get; set; }

        [JsonIgnore]
        [ForeignKey("storeId")]
        public virtual Store store { get; set; }

        [JsonIgnore]
        [ForeignKey("customerId")]
        public virtual Customer customer { get; set; }
    }
}
