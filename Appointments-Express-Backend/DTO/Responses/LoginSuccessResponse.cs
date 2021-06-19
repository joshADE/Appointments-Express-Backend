using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class LoginSuccessResponse : LoginResponse
    {
        public string token { get; set; }
        public UserResponse user { get; set; }

    }
}
