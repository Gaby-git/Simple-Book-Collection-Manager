using Domaine.Model;

namespace Domaine.Service
{
    public interface IGenreService
    {
        public Genre AddGenre(Genre genre);

        public Genre UpdateGenre(int genreId, Genre updatedGenre);

        public Genre DeleteGenre(int genreId);

        public Genre GetGenreById(int genreId);

        public IEnumerable<Genre> GetAllGenres();
    }
}
