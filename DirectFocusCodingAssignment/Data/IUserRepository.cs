
using DirectFocusCodingAssignment.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectFocusCodingAssignment.Data
{
    public interface IUserRepository:IDisposable
    {
        List<User> GetUsers();
        User GetUserByID(int userId);
        void CreateUser(User user);
        void DeleteUser(int userID);
        void UpdateUser(User user);
        bool SaveAll();

    }
}
