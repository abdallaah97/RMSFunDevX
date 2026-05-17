using Application.Repositories;
using Infrastructre.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //Dependancy Injection
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Delete(T input)
        {
            _dbSet.Remove(input);
        }

        public IQueryable<T> GetAll()
        {
            var data = _dbSet.AsQueryable();
            return data;
        }

        public IEnumerable<T> GetAllList()
        {
            var data = _dbSet.ToList();
            return data;
        }

        public void Update(T input)
        {
            _dbSet.Update(input);
        }
        public T GetById(int id)
        {
            var data = _dbSet.Find(id);
            return data;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var data = await _dbSet.FindAsync(id);
            return data;
        }

        public void Insert(T input)
        {
            _dbSet.Add(input);
        }

        public async Task InsertAsync(T input)
        {
            await _dbSet.AddAsync(input);
        }

        public void InsertRange(List<T> input)
        {
            _dbSet.AddRange(input);
        }

        public async Task InsertRangeAsync(List<T> input)
        {
            await _dbSet.AddRangeAsync(input);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
