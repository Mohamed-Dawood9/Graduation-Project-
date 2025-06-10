using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
    public class Analysis : ModelBase
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string? OriginalPhotoPath { get; set; }
        public string? ProcessedPhotoPath { get; set; }
        public ICollection<Note>? Notes { get; set; } = new HashSet<Note>();
        public ICollection<Keypoint> Keypoints { get; set; } = new HashSet<Keypoint>();
        public float HDI_S { get;  set; }
        public float HDI_A { get;  set; }
        public float HDI_T { get;  set; }
        public float FAI_C7 { get;  set; }
        public float FAI_A { get;  set; }
        public float FAI_T { get;  set; }
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

    }
}
