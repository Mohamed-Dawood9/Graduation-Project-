using GP.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
	public interface IPatientsInterface: IGenericInterface<Patient>
	{
        IQueryable<Patient> GetEmployeeByName(string name);
    }
}
