using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Services
{
    public interface IUserService
    {
        User Register(User oUser);

        User Login(string username, string password);

        User EditAccount(User oUser);

        List<User> GetAllUsers();
    }
}
