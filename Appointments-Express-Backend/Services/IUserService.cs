using Appointments_Express_Backend.DTO.Requests;
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

        EditAccountRequest EditAccount(EditAccountRequest oUser);

        List<User> GetAllUsers();
    }
}
