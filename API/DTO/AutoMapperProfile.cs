using API.Entities;
using AutoMapper;

namespace API.DTO
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();

            CreateMap<Author, AuthorDTO>().ReverseMap();

           
        }
    }
}
