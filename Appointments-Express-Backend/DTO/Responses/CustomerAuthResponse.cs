using Appointments_Express_Backend.DTO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class CustomerAuthResponse
    {
        public CustomerResponse customer { get; set; }
        public CustomerAuthRequest request { get; set; }
    }
}
