using GP.DAL.Data.Models;

namespace GP.PL.VIewModel
{
    public class NoteViewModel
    {
       
            public int Id { get; set; }
            public string Content { get; set; }
            public int AppointmentId { get; set; }
            public Appointment Appointment { get; set; } // Navigation Property
        
    }
}
