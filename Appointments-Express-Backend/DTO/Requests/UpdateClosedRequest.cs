using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class UpdateClosedRequest
    {
        public ClosedDaysTimes[] toAdd { get; set; }
        public ClosedDaysTimes[] toUpdate { get; set; }
        public ClosedDaysTimes[] toRemove { get; set; }
    }
}
