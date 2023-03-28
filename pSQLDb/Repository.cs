using Microsoft.EntityFrameworkCore;

namespace GBM.psqlDB
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly psqlContext _context;
        private DbSet<T> _entities;

        public Repository(psqlContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public T GetById(Guid id)
        {
            return _entities.Find(id);
        }

        public List<T> GetAll()
        {
            return _entities.ToList();
        }
    }
}