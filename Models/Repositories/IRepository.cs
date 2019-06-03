using System.Collections.Generic;
using System.Linq;
using SimpleWebsite.Models.Base;

namespace SimpleWebsite.Models.Repositories
{
    public interface IRepository<T> where T : EntityBase, new()
    {
        IQueryable<T> Entities { get; }
        T this[int id] { get; }
        int Add(T entity);
        T Delete(int id);
        T Delete(T entity);
        int Update(T entity);
    }
}