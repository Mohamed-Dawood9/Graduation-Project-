using GP.DAL.Data.Models;

namespace GP.PL.VIewModel
{
    public class PatientAppointmentViewModel
    {
        public Patient Patient { get; set; }
        public IEnumerable<AppointmentViewModel> Appointments { get; set; }
        

    }
}
