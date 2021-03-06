using Appointments_Express_Backend.DTO.Responses;
using Appointments_Express_Backend.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Mapping
{
    public class ModelToResponse : Profile
    {
        public ModelToResponse()
        {
            CreateMap<User, UserResponse>();

            CreateMap<Customer, CustomerResponse>();
        }
    }
}
