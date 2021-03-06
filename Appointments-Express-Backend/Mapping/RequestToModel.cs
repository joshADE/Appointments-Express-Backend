using Appointments_Express_Backend.DTO.Requests;
using Appointments_Express_Backend.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointments_Express_Backend.Mapping
{
    public class RequestToModel : Profile
    {
        public RequestToModel()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<EditAccountRequest, User>()
                .ForAllMembers(opt => opt.Condition((src, des, srcMember) => srcMember != null));
        }
    }
}
