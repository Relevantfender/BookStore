using API.Entities;
using AutoMapper;

namespace API.DTO
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<BookDTO, Book>()
             .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors))
             .ReverseMap();

            CreateMap<AuthorDTO, Author>()
                .ReverseMap();

            CreateMap<ICollection<AuthorDTO>, ICollection<Author>>()
             .ConvertUsing((src, dest, context) =>
                 context.Mapper.Map<ICollection<Author>>(src));

            CreateMap<ICollection<Author>, ICollection<AuthorDTO>>()
                .ConvertUsing((src, dest, context) =>
                    context.Mapper.Map<ICollection<AuthorDTO>>(src));
        }
    }
}
