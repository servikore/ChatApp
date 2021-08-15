using Domain.Entities;
using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EFBaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected AppDbContext context { private set; get; }
        protected DbSet<T> entities { private set; get; }

        public EFBaseRepository(AppDbContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return;
            }

            entity.DetetedOn = DateTime.UtcNow;
            entities.Remove(entity);
        }

        public Task<List<T>> GetAllAsync()
        {
            return this.entities.ToListAsync();
        }

        public Task<T> GetByIdAsync(int id)
        {
            return this.entities.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertAsync(T obj)
        {
            obj.CreateOn = DateTime.UtcNow;            
            await this.entities.AddAsync(obj);
        }
        public Task UpdateAsync(T obj)
        {
            return this.context.SaveChangesAsync();
        }

        public Task SaveAsync()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
