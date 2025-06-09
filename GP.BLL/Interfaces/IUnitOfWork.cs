using GP.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

       public IPatientsInterface PatientsRepositry { get; set; }
       public IAppointmentInterface AppointmentsRepositry { get; set; }
       public INotesInterface NotesRepositry { get; set; }

       public IAnalysisInterFace AnalysisRepositry { get; set; }
        int Complete();
        
    }
}
