using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Utils
{
    public class MailSender : IMailSender
    {

        private readonly IServiceProvider _serviceProvider;

        public MailSender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void SendAppointmentConfirmationEmail(string recipientEmail, string recipientName, string content)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

                var email = mailer
                    .To(recipientEmail)
                    .Subject("Appointment Link")
                    .Body(content);
                    


                await email.SendAsync();
            }
        }
    }
}
