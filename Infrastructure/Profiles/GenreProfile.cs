using AutoMapper;

namespace Infrastructure.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile ()
        {
            CreateMap<Entities.Genre, Domaine.Model.Genre>();

        }
    }
}
