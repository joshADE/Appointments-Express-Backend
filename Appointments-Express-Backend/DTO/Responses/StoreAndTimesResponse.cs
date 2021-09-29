using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Responses
{
    public class StoreAndTimesResponse
    {
        public Store store { get; set; }
        public IEnumerable<StoreHours> storeHours { get; set; }
        public IEnumerable<ClosedDaysTimes> closedDaysTimes { get; set; }
    }
}
