using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : EFBaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
        public Task<User> GetByEmailAsync(string email)
        {
            return this.entities.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
