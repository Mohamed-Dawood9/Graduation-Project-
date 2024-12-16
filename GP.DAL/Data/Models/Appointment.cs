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
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }
    }
}
