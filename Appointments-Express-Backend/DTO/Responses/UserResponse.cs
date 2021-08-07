using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class UserResponse
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string avatarPublicId { get; set; }
        public string avatarUrl { get; set; }
        public DateTime createdAt { get; set; }
    }
}
