using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public UserRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid id)
            => await _context.Users.FindAsync(id);

        public async Task<User> GetByUsernameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        public async Task<User> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

        public async Task UpdateAsync(User user)
            => _context.Users.Update(user);

        public async Task<bool> UserExists(string userName, string email)
            => await _context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower() && u.Email.ToLower() == email.ToLower());
    }
}
