using GP.DAL.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; } // Navigation Property

        public DateTime Date { get; set; }=DateTime.Now;
        public ICollection<Note>? Notes { get; set; } = new List<Note>();

        public IFormFile? Image { get; set; }

        public string? OrginalPhotoPath { get; set; }
        public string? ProcessedPhotoPath1 { get; set; }
        public string? ProcessedPhotoPath2 { get; set; }
        public string? ProcessedPhotoPath3 { get; set; }
        [Display(Name = "Cobb Angle")]
        [Range(0, 180, ErrorMessage = "Cobb Angle must be between 0 and 180.")]
        public decimal CobbAngle { get; set; } 
        public string? Diagnosis { get; set; }
        public string? NewNote { get; set; }
        public AnalysisViewModel? Analysis { get; set; } // Added navigation property

    }
}
