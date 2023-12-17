namespace MovieAPI.Application.Interfaces {
    public interface IGenericRepository<T> where T : class {
        Task<T> GetAsync(int id);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
