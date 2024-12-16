using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class Measurement : ModelBase
	{
        public int ProccessedImageId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
    }
}
