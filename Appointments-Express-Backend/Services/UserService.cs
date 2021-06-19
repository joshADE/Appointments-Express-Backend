using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly AppointmentDBContext _context;

        public UserService(AppointmentDBContext context)
        {
            _context = context;
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.username == username);
            if (user == null) throw new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.NotFound);
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.password);

            if (isValidPassword)
            {
                return user;
            }
            return null;

        }

        public User Register(User oUser)
        {
            oUser.password = BCrypt.Net.BCrypt.HashPassword(oUser.password);
            // TODO: Should add the user in the database here
            return oUser;
        }
    }
}
