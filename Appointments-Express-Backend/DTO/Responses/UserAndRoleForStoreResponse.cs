using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class UserAndRoleForStoreResponse
    {
        public UserResponse user { get; set; }
        public Role role { get; set; }
        public Store store { get; set; }
        public DateTime createdAt { get; set; }
    }
}
