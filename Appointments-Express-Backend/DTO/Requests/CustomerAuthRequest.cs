using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class CustomerAuthRequest
    {
        public int customerId { get; set; }
        public string passcode { get; set; }
    }
}
