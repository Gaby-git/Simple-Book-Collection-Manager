using Domaine.Model;
using Domaine.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class UserService : IUserService
    {
        public User AddUser (User user)
        {
            return default (User);
        }

        public IEnumerable<User> GetUser (User user)
        {
            return default(IEnumerable<User>);
        }

        public User GetUserById(int userId)
        {
            return default;
        }

        public User UpdateUser(int userId, User updatedUser)
        {
            return default;
        }

        public User DeleteUser(int userId)
        {
            return default;
        }
    }
}
