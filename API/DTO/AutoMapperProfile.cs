using API.Entities;
using AutoMapper;

namespace API.DTO
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Book, BookDTO>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<BookDTO, Book>();
            CreateMap<AuthorDTO, Author>();
        }
    }
}
