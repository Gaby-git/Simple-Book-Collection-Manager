namespace Infrastructure.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Entities.Book, Domaine.Model.Books>();

        }
    }
}
