using System.Collections.Generic;
using System.Linq;
using SimpleWebsite.Models.Base;

namespace SimpleWebsite.Models.Repositories
{
    public class MemoryBaseRepository<T> : IRepository<T> where T : EntityBase, new()
    {
        
        private List<T> _entities = new List<T>();

        public T this[int id] => _entities.FirstOrDefault(e => e.Id == id);

        public IQueryable<T> Entities => _entities.AsQueryable();

        public int Add(T entity)
        {
            if (_entities.Any())
            {
                entity.Id = Entities.Max(e => e.Id) + 1;
            }
            else
            {
                entity.Id = 1;
            }
            
            _entities.Add(entity);
            return 1;
        }

        public T Delete(int id)
        {
            T entry = _entities.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                _entities.Remove(entry);
            }
            return entry;
        }

        public T Delete(T entity)
        {
            T entry = _entities.FirstOrDefault(e => e.Id == entity.Id);
            if (entry != null)
            {
                _entities.Remove(entry);
            }
            return entry;
        }

        public int Update(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}