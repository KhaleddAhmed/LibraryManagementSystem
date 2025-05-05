using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Core.DTOs.Book;
using LibraryManagementSystem.Core.DTOs.Category;
using LibraryManagementSystem.Core.DTOs.Librarian;
using LibraryManagementSystem.Core.DTOs.User;
using LibraryManagementSystem.Core.Entities.Library;
using LibraryManagementSystem.Core.Entities.User;

namespace LibraryManagementSystem.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapUser();
            MapLibrarian();
            MapCategory();
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

        private void MapCategory()
        {
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, GetAllCategoriesDto>();
            CreateMap<Category, GetCategoryDto>();
        }

        private void MapBook()
        {
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();
            CreateMap<Book, GetAllBooksDto>();
            CreateMap<Book, GetBookDto>()
                .ForMember(dest => dest.CategoryName, o => o.MapFrom(src => src.Category.Name));
        }
    }
}
