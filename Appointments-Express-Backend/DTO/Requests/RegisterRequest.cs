using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class RegisterRequest
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public IFormFile avatar { get; set; }
    }
}
