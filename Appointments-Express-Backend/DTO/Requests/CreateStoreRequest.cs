using Appointments_Express_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.DTO.Requests
{
    public class CreateStoreRequest
    {
        public Store store { get; set; }

        public IEnumerable<StoreHours> storeHours { get; set; }

        public IEnumerable<ClosedDaysTimes> closedDaysTimes { get; set; }
    }
}
