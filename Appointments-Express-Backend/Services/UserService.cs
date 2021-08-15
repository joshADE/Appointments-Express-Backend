using Appointments_Express_Backend.DTO.Requests;
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
            username = username.ToLower();
            var user = _context.Users.FirstOrDefault(u => u.username == username);
            if (user == null) return null;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.password);

            if (isValidPassword)
            {
                return user;
            }
            return null;

        }

        public User Register(User oUser)
        {
            oUser.username = oUser.username.ToLower();
            oUser.email = oUser.email.ToLower();
            oUser.password = BCrypt.Net.BCrypt.HashPassword(oUser.password);
            // TODO: Should add the user in the database here
            return oUser;
        }

        public EditAccountRequest EditAccount(EditAccountRequest oUser)
        {
            if (oUser.username != null)
                oUser.username = oUser.username.ToLower();

            if (oUser.email != null)
                oUser.email = oUser.email.ToLower();

            if (oUser.password != null)
                oUser.password = BCrypt.Net.BCrypt.HashPassword(oUser.password);
            // TODO: Should add the user in the database here
            return oUser;
        }
    }
}
