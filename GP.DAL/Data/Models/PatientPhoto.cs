using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class PatientPhoto : ModelBase
	{
        public int AppointmentId { get; set; }
		public string Path { get; set; }
    }
}
