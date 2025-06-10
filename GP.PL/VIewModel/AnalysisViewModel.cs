using GP.DAL.Data.Models;

namespace GP.PL.VIewModel
{
    public class AnalysisViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        public string? OriginalPhotoPath { get; set; }
        public string? ProcessedPhotoPath { get; set; }
        public string? NewNote { get; set; }
        public ICollection<Note> Notes { get; set; } = new List<Note>(); // Add Notes collection
        public float HDI_S { get; set; }
        public float HDI_A { get; set; }
        public float HDI_T { get; set; }
        public float FAI_C7 { get; set; }
        public float FAI_A { get; set; }
        public float FAI_T { get; set; }
        public float TotalHDI { get; set; }
        public float TotalFAI { get; set; }
        public float POTSI { get; set; }
        public float LeftSideDistance { get; set; }
        public float RightSideDistance { get; set; }
        public float LeftUnderArmDistance { get; set; }
        public float RightUnderArmDistance { get; set; }
    }
}
