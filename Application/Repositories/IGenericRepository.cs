namespace Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public IEnumerable<T> GetAllList();
        public void Delete(T input);
        public void DeleteRange(IEnumerable<T> input);
        public void Update(T input);
        public T GetById(int id);
        public Task<T> GetByIdAsync(int id);
        public void Insert(T input);
        public Task InsertAsync(T input);
        public void InsertRange(List<T> input);
        public Task InsertRangeAsync(List<T> input);
        public void SaveChanges();
        public Task SaveChangesAsync();
    }
}
