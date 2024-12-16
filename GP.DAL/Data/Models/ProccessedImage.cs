using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class ProccessedImage :ModelBase
	{
        public int PatientId { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }
}
