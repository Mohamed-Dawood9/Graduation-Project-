using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
    public class Keypoint : ModelBase
    {
        public int AnalysisId { get; set; }
        public Analysis Analysis { get; set; }
        public string Name { get; set; } // e.g., "C7", "T7", "LeftHip"
        public float X { get; set; }
        public float Y { get; set; }
    }
}
