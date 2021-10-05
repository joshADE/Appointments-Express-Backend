using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Utils
{
    public interface IMailSender
    {

        void SendAppointmentConfirmationEmail(string recipientEmail, string recipientName, string content);
    }
}
