using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.Entities.User;

namespace LibraryManagementSystem.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, AppUser>();
        }
    }
}
