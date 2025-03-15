//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GP.DAL.Data.Models
//{
//	public class PatientPhoto : ModelBase
//	{
//		//foregin key for appointment
//        public int AppointmentId { get; set; }
//		public string Path { get; set; }
//		public Appointment Appointment { get; set; } // Navigation Property
//		public ICollection<ProcessedImage> ProcessedImages { get; set; } = new HashSet<ProcessedImage>();
//	}
//}
