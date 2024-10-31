
namespace Domaine.Model
{
    public class BookStatistics
    {
        public int TotalBooks { get; set; }
        public int BooksRead { get; set; }
        public int BooksUnread { get; set; }
        public string FavoriteGenre { get; set; } // Based on the highest number of books
    }

}
