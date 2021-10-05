using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appointments_Express_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Appointments_Express_Backend.DTO.Requests;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using FluentEmail.Smtp;
using FluentEmail.Core;
using System.Net;
using Appointments_Express_Backend.Utils;

namespace Appointments_Express_Backend.Controllers.api
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentDBContext _context;
        private readonly IConfiguration _config;
        private readonly IMailSender _mailSender;

        public AppointmentsController(AppointmentDBContext context, IConfiguration config, IMailSender mailSender)
        {
            _context = context;
            _config = config;
            _mailSender = mailSender;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // GET: api/Appointments/storeappointments/5
        [HttpGet("storeappointments/{storeId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetStoreAppointments(int storeId)
        {
            return await _context.Appointments.Where(app => app.storeId == storeId).ToListAsync();
        }

        // POST: api/Appointments/createappointment
        [HttpPost("createappointment")]
        public async Task<ActionResult<Appointment>> CreateAppointmentAndCustomerIfNotExist([FromBody] CreateAppointmentRequest request)
        {

            var email = request.email.ToLower();
            var existingUser = await _context.Customers.FirstOrDefaultAsync(cus => cus.email == email);
            var userExist = existingUser != null;

            var appointment = request.appointment;
            var customer = existingUser;



            Random rnd = new Random();
            var passcode = (rnd.Next(100000, 999999)).ToString();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    

                    if (!userExist)
                    {
                        
                        customer = new Customer { email = request.email.ToLower(), firstName = request.firstName, lastName = request.lastName, password = BCrypt.Net.BCrypt.HashPassword(passcode) };
                        _context.Customers.Add(customer);
                    }
                    else
                    {
                        // update the customers info and the passcode to be recent
                        customer.firstName = request.firstName;
                        customer.lastName = request.lastName;
                        customer.password = BCrypt.Net.BCrypt.HashPassword(passcode);
                        _context.Entry(customer).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();

                    

                    appointment.customerId = customer.id;
                    appointment.status = Appointment.AppointmentStatus.pending;
                    // store id of the appointment is set already by the client
                    _context.Appointments.Add(appointment);
                    await _context.SaveChangesAsync();


                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("A database error has occured", ex);
                }
            }

            // send email to the customer
            var link = request.domain + $"/customer/{customer.id}/appointments";
            _mailSender.SendAppointmentConfirmationEmail(customer.email, customer.firstName + " " + customer.lastName, "Link to Appointment: " + link + " Passcode: " + passcode);

            return CreatedAtAction("GetAppointment", new { id = appointment.id }, appointment);
        }

        // POST: api/Appointments/5/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{storeId}/{customerId}")]
        public async Task<ActionResult<Appointment>> PostAppointment([FromRoute] int storeId, [FromRoute] int customerId, [FromBody] Appointment appointment)
        {
            appointment.customerId = customerId;
            appointment.storeId = storeId;
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointment.id }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.id == id);
        }



    }
}
