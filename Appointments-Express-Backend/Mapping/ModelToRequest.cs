using Appointments_Express_Backend.DTO.Requests;
using Appointments_Express_Backend.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Mapping
{
    public class ModelToRequest : Profile
    {
        public ModelToRequest()
        {
            CreateMap<User, EditAccountRequest>();
        }
    }
}
