using Microsoft.EntityFrameworkCore;
using Product.Core.Interface;
using Product.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository
{
    public class GenergicRepository<T> : IGenergicRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;
        public GenergicRepository(ApplicationDbContext context) 
        {
            this._context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public  IEnumerable<T> GetAll() 
        => _context.Set<T>().AsNoTracking().ToList();


        public  IEnumerable<T> GetAll(params Expression<Func<T, bool>>[] includes)
        =>  _context.Set<T>().AsNoTracking().ToList();

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);

            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByidAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var item in includes)
            {
                query= query.Include(item);
            }
            return await ((DbSet<T>)query).FindAsync(id);
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var entity_value = await _context.Set<T>().FindAsync(id);
            if(entity_value != null)
            {
                _context.Update(entity_value);
                await _context.SaveChangesAsync();
            }
        }
    }
}
