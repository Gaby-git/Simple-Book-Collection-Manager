namespace Infrastructure.Profiles
{
    public class GenreProfile
    {
        public GenreProfile ()
        {
            CreateMap<Entities.Genre, Domaine.Model.Genre>();

        }
    }
}
