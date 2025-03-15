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
    public class AppointmentRepositry:GenericRepository<Appointment>,IAppointmentInterface
    {
        public AppointmentRepositry(AppDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
