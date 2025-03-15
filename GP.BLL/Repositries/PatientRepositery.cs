using Demo.BLL.Repositories;
using GP.BLL.Interfaces;
using GP.DAL.Data;
using GP.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositries
{
    public class PatientRepositery : GenericRepository<Patient>, IPatientsInterface
    {
        public PatientRepositery(AppDbContext dbContext):base(dbContext)
        {
            
        }
        IQueryable<Patient> IPatientsInterface.GetEmployeeByName(string name)
        {
            return _DbContext.Patients.Where(e => e.Name.ToLower().Contains(name));
        }
    }
}
