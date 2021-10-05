using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class CreateAppointmentRequest
    {
        public Appointment appointment { get; set; }

        public string email { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }

        public string domain { get; set; }


    }
}
