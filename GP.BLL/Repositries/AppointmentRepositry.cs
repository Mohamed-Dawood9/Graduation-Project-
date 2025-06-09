using Demo.BLL.Repositories;
using GP.BLL.Interfaces;
using GP.DAL.Data;
using GP.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositries
{
    public class AppointmentRepositry:GenericRepository<Appointment>,IAppointmentInterface
    {
        private readonly AppDbContext _dbContext;
        public AppointmentRepositry(AppDbContext dbContext):base(dbContext)
        {
            
        }
        public Appointment GetById(int id)
        {
            return _DbContext.Appointments
                .Include(a => a.Notes) // Eagerly load Notes
                .FirstOrDefault(a => a.Id == id);
        }

        public IQueryable<Appointment> GetAll()
        {
            return _DbContext.Appointments
                .Include(a => a.Notes); // Eagerly load Notes for GetAll
        }

    }
}
