using GP_BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP_BLL.Interfaces
{
    public interface ISpineProcessingService
    {
        void ProcessSpine(string glbPath, string stlPath, string outputDirectory);
        Task<MedicalReport> ProcessBackImageAsync(string imagePath, string outputFolder);
        void AnnotateAndSaveImage(MedicalReport report, string inputImagePath, string outputImagePath);
    }
}
