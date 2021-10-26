using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class AppointmentAndStoreResponse
    {
        public Appointment appointment { get; set; }
        public Store store { get; set; }
    }
}
