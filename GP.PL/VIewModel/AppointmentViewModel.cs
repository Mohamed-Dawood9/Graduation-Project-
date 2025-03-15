using GP.DAL.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } // Navigation Property

        public DateTime Date { get; set; }=DateTime.Now;
        public ICollection<Note> Notes { get; set; } = new HashSet<Note>();


        public string? OrginalPhotoPath { get; set; }
        public string? ProccessedPhotoPath { get; set; }
        public decimal? CobbAngle { get; set; }
        public string? Diagnosis { get; set; }
    }
}
