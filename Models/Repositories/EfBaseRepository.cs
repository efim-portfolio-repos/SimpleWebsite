using System.Linq;
using System.Collections.Generic;
using SimpleWebsite.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace SimpleWebsite.Models.Repositories
{
    public class EfBaseRepository<T> : IRepository<T> where T : EntityBase, new()
    {
        private ApplicationDbContext _context;
        private DbSet<T> _table;
        public EfBaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }
        internal int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public T this[int id] => _table.FirstOrDefault(e => e.Id == id);

        public virtual IQueryable<T> Entities => _table;

        public int Add(T entity)
        {
            entity.Id = 0;
            _table.Add(entity);
            return SaveChanges();
        }

        public T Delete(int id)
        {
            T dbEntry = _table.FirstOrDefault(e => e.Id == id);

            if (dbEntry != null)
            {
                _table.Remove(dbEntry);
                SaveChanges();
            }

            return dbEntry;
        }

        public T Delete(T entity)
        {
            return Delete(entity.Id);
        }

        public int Update(T entity)
        {
            if (_table.Any(e => e.Id == entity.Id))
            {
                _table.Update(entity);
            }

            return SaveChanges();
        }
    }
}