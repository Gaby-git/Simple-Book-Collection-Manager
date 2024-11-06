using AutoMapper;
using Domaine.Model;

namespace Infrastructure.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Entities.Book, Book>();

        }
    }
}
