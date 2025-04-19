using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.Entities.User;

namespace LibraryManagementSystem.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapUser();
            MapLibrarian();
        }

        private void MapUser()
        {
            CreateMap<RegisterDto, AppUser>();
        }

        private void MapLibrarian()
        {
            CreateMap<CreateLibrarianDto, AppUser>();
            CreateMap<UpdateLibrarianDto, AppUser>();
            CreateMap<AppUser, GetAllLibrarianDto>();
            CreateMap<AppUser, GetLibrarianDto>();
        }
    }
}
