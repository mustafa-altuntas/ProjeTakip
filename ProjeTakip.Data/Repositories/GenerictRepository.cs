using Microsoft.EntityFrameworkCore;
using ProjeTakip.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjeTakip.Data.Repositories
{
    public class GenerictRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {
        private readonly DbContext _context;
        private DbSet<Tentity> _dbSet;

        public GenerictRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<Tentity>();
        }



        public async Task AddAsync(Tentity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public IQueryable<Tentity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        //public async Task<IEnumerable<Tentity>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        public async Task<Tentity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity is not null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task Remove(Tentity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task Update(Tentity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<Tentity> Where(Expression<Func<Tentity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}
