using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class EditAvatarRequest
    {
        public IFormFile avatar { get; set; }
    }
}
