namespace GBM.psqlDB
{
    // Interface for data to be stored.
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(Guid id);
        List<T> GetAll();
    }
}
