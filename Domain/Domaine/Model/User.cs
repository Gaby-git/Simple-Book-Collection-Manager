
namespace Domaine.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Book> Books { get; set; } // Collection of books associated with the user
    }

}
