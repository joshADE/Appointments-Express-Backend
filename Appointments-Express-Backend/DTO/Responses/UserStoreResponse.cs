using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class UserStoreResponse
    {
        public int id { get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public int minTimeBlock { get; set; }

        public int maxTimeBlock { get; set; }

        public bool isQuickProfile { get; set; }

        public DateTime createdAt { get; set; }

        // new fields to describe the users role for the store
        public string role { get; set; }
    }
}
