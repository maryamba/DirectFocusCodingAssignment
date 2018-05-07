using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectFocusCodingAssignment.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DirectFocusCodingAssignment.Data
{
    public class UserRepository : IUserRepository, IDisposable    {

        private readonly DFCContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DFCContext context,ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void CreateUser(User user)
        {
            try
            {

                _context.Users.Add(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create user: {ex}");
            }
        }

        public void DeleteUser(int userID)
        {
            try
            {
                User user = _context.Users.Find(userID);
                _context.Users.Remove(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete user: {ex}");               
            }
        }

        public User GetUserByID(int userId)
        {
            try
            {
                return _context.Users.Find(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get user by user Id: {ex}");
                return null;
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                if (_context.Users != null)
                    return _context.Users.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all users: {ex}");
                return null;
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update user: {ex}");               
            }

        }

        public bool SaveAll()
        {
            try
            {
               return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save user context {ex}");
                return false;
            }

        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
