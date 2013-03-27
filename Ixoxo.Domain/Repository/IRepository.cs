using System.Collections.Generic;

namespace Ixoxo.Domain.Repository
{
    public interface IRepository<T, TID>
    {
        T Get(TID id);
        T GetByIdAndLock(TID id);
        IList<T> GetAll();
        IList<T> GetAll(int pageNumber, int pageSize, out int totalPages);
        T Save(T entity);
        void Delete(T entity);
        void Refresh(T entity);
        void FlushSession();
    }
}