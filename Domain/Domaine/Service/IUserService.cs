﻿using Domaine.Model;

namespace Domaine.Service
{
    public interface IUserService
    {
        public User AddUser(User user);

        public User GetUserById(int userId);

        public User UpdateUser(int userId, User updatedUser);

        public User DeleteUser(int userId);

        public IEnumerable<Book> GetUserBooks(int userId);
    }
}