using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Appointments_Express_Backend.Models.Appointment;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class UpdateAppointmentStatusRequest
    {
        public int appointmentId { get; set; }
        public AppointmentStatus newStatus { get; set; }
    }
}
