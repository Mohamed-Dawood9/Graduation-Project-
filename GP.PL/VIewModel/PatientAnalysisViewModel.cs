using GP.DAL.Data.Models;

namespace GP.PL.VIewModel
{
    public class PatientAnalysisViewModel
    {
        public Patient Patient { get; set; }
        public IEnumerable<AnalysisViewModel> Analyses { get; set; }
    }
}
