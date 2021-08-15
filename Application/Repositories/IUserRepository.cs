﻿using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
