using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class Appointment : ModelBase
	{
		// Foreign Key for Patient
		public int PatientId { get; set; }
		public Patient Patient { get; set; } // Navigation Property

		// Foreign Key for Doctor
		public int DoctorId { get; set; }
		//public Doctor Doctor { get; set; } // Navigation Property

		public DateTime Date { get; set; }
        public ICollection<Note> Notes { get; set; } = new HashSet<Note>();

       public string OrginalPhotoPath { get; set; }
       public string ProccessedPhotoPath { get; set; }
       public decimal CobbAngle { get; set; }
       public string Diagnosis { get; set; }



    }
}
