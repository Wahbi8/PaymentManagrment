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

        public async Task AddUser(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task RemoveUser(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if(user == null) throw new BusinessException("Can't find the user");

            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
