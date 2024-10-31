using Domaine.Model;

namespace Domaine.Service
{
    public interface IBookService
    {
        public Book AddBook(Book book);

        public Book UpdateBook(int bookId, Book updatedBook);

        public Book DeleteBook(int bookId);

        public Book GetBookById(int bookId);

        public IEnumerable<Book> GetAllBooks();

        public IEnumerable<Book> GetBooksByReadStatus(bool isRead);

        public IEnumerable<Book> GetBooksByGenre(string genre);

        public IEnumerable<Book> SearchBooks(string searchTerm);

        public BookStatistics GetBookStatistics();
    }
}
