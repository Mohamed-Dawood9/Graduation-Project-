
using GP.BLL.Interfaces;
using GP.BLL.Repositries;
using GP.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IPatientsInterface PatientsRepositry { get; set; }
        public IAppointmentInterface AppointmentsRepositry { get; set; }
        public INotesInterface NotesRepositry { get; set; }
        public IAnalysisInterFace AnalysisRepositry { get; set; }


        public UnitOfWork(AppDbContext dbContext)
        {
            PatientsRepositry = new PatientRepositery(dbContext);
            AppointmentsRepositry = new AppointmentRepositry(dbContext);
            NotesRepositry =new NotesRepositry(dbContext);
            AnalysisRepositry = new AnalysisRepositry(dbContext);
            _dbContext = dbContext;
        }
        public int Complete()
        {
           return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
             _dbContext.Dispose();
        }

    }
}
