using GP.BLL.Interfaces;
using GP.DAL.Data;
using GP.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericInterface<T> where T : ModelBase
    {
        private protected readonly AppDbContext _DbContext;
        public GenericRepository(AppDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Appointment))
            {
                return (IEnumerable<T>)_DbContext.Appointments.Include(e => e.Notes).AsNoTracking().ToList();
            }
            return _DbContext.Set<T>().AsNoTracking().ToList();
        }

        public T GetById(int id)
        
           => _DbContext.Find<T>(id);
        

        public void Add(T entity)
        {
            _DbContext.Set<T>().Add(entity);
			 //_DbContext.SaveChanges();
			//_DbContext.Add(entity);

		}

        public void Delete(T entity)
        {

            _DbContext.Remove(entity);
			//_DbContext.SaveChanges();

		}


        public void Update(T entity)
        {
            _DbContext.Update(entity);
			//_DbContext.SaveChanges();

		}

    }
}
