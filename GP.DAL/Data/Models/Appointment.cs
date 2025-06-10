using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
    public class Appointment : ModelBase
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }

        public DateTime Date { get; set; }
        public ICollection<Note>? Notes { get; set; } = new HashSet<Note>();

        public string? OrginalPhotoPath { get; set; }
        public string? ProcessedPhotoPath1 { get; set; } // First processed image
        public string? ProcessedPhotoPath2 { get; set; } // Second processed image
        public string? ProcessedPhotoPath3 { get; set; } // Third processed image
        public decimal? CobbAngle { get; set; }
        public string? Diagnosis { get; set; }
        public Analysis Analysis { get; set; }
    }
}
