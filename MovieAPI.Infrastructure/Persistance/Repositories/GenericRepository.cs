using Microsoft.EntityFrameworkCore;
using MovieAPI.Application.Interfaces;

namespace MovieAPI.Infrastructure.Persistance.Repositories {
    public class GenericRepository<T> : IGenericRepository<T> where T : class {
        protected readonly DbContext Context;
        private readonly DbSet<T> _entity;

        public GenericRepository(DbContext context) {
            Context = context;
            _entity = context.Set<T>();
        }

        public async Task AddAsync(T entity) {
            await _entity.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities) {
            await _entity.AddRangeAsync(entities);
        }

        public async Task<T> GetAsync(int id) {
            return await _entity.FindAsync(id);
        }

        public void Remove(T entity) {
            _entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities) {
            _entity.RemoveRange(entities);
        }
    }
}
