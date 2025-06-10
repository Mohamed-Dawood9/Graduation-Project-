using GP.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
	public interface IAppointmentInterface : IGenericInterface<Appointment>
	{
        //IQueryable<Appointment> GetEmployeesByDate(DateTime date);
        IQueryable<Appointment> GetAllWithAnalysis();

    }
}
