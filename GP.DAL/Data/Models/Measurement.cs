using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class Measurement : ModelBase
	{
       
        public string Type { get; set; }
        public decimal Value { get; set; }

		// Foreign Key for ProcessedImage
		public int ProcessedImageId { get; set; }
		public ProcessedImage ProcessedImage { get; set; }
	}
}
