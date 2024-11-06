using Domaine.Model;

namespace Domaine.Stores
{
    public interface IUserStore
    {
        public IEnumerable<User> GetUsers();
    }
}
