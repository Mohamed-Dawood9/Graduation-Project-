using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
    public class Note:ModelBase
    {
        
            public string Content { get; set; } // The actual note text

        // Foreign Key for Appointment
        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        // Foreign Key for Analysis (nullable)
        public int? AnalysisId { get; set; }
        public Analysis Analysis { get; set; }


    }
}
