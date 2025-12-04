using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentManagement.Domain;

namespace PaymentManagement.Infrastructure
{
    public  class UserRepository
    {
        private readonly AppDbContext context;
        public UserRepository(AppDbContext connection)
        {
            context = connection;
        }

        public async Task<List<User>> GetAllUsers() => await context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(Guid id) => await context.Users.FindAsync(id);

        public async Task<bool> AddUser(User user)
        {
            await context.Users.AddAsync(user);
            int result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RemoveUser(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if(user == null) return false;

            context.Users.Remove(user);
            int result = await context.SaveChangesAsync();
            return result > 0;
        }
    }
}
