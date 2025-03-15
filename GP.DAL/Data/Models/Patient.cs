using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class Patient:ModelBase
	{
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MedicalHistory { get; set; }
		// One-to-Many with Appointments
		public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
	}
}
