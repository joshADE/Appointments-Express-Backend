using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.AuthenticationManager
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(User user);
    }
}
