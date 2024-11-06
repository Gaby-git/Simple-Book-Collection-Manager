using AutoMapper;
using Domaine.Model;

namespace Infrastructure.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, User>();
        }
    }
}
