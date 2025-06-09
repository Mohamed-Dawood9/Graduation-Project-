using AutoMapper;
using GP.BLL.Interfaces;
using GP.DAL.Data.Models;
using GP.PL.Helper;
using GP.PL.VIewModel;
using GP_BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GP.PL.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class AnalysisController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISpineProcessingService _spineProcessingService;

        public AnalysisController(IUnitOfWork unitOfWork, IMapper mapper, ISpineProcessingService spineProcessingService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _spineProcessingService = spineProcessingService;
        }

        public IActionResult Index(int patientId)
        {
            var patient = _unitOfWork.PatientsRepositry.GetById(patientId);
            if (patient == null)
                return NotFound();

            var analyses = _unitOfWork.AnalysisRepositry
                .GetAll()
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.Date)
                .ToList();

            var mappedAnalyses = _mapper.Map<IEnumerable<Analysis>, IEnumerable<AnalysisViewModel>>(analyses);

            var viewModel = new PatientAnalysisViewModel
            {
                Patient = patient,
                Analyses = mappedAnalyses
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(int patientId)
        {
            var viewModel = new AnalysisViewModel
            {
                PatientId = patientId
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnalysisViewModel analysisVm)
        {
            if (ModelState.IsValid)
            {
                // Upload the original .jpg file
                analysisVm.OriginalPhotoPath = DocumentSettings.UpdloadFile(analysisVm.Image, "images");
                var mappedAnalysis = _mapper.Map<AnalysisViewModel, Analysis>(analysisVm);

                // Save the analysis to get the Analysis.Id
                _unitOfWork.AnalysisRepositry.Add(mappedAnalysis);
                var count = _unitOfWork.Complete();
                if (count <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Failed to save the analysis.");
                    return View(analysisVm);
                }

                // Create folder structure
                string patientFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "processed_images", $"Patient_{analysisVm.PatientId}");
                string analysisFolder = Path.Combine(patientFolder, $"Analysis_{mappedAnalysis.Id}");
                Directory.CreateDirectory(analysisFolder);

                // Process the image
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", analysisVm.OriginalPhotoPath);
                if (!System.IO.File.Exists(imagePath))
                {
                    ModelState.AddModelError(string.Empty, $"Image file not found at: {imagePath}.");
                    return View(analysisVm);
                }

                try
                {
                    var report = await _spineProcessingService.ProcessBackImageAsync(imagePath, analysisFolder);
                    string outputImagePath = Path.Combine(analysisFolder, "AnnotatedImage.jpg");
                    _spineProcessingService.AnnotateAndSaveImage(report, imagePath, outputImagePath);

                    mappedAnalysis.ProcessedPhotoPath = $"/processed_images/Patient_{analysisVm.PatientId}/Analysis_{mappedAnalysis.Id}/AnnotatedImage.jpg";
                    mappedAnalysis.HDI_S = report.HDI_S;
                    mappedAnalysis.HDI_A = report.HDI_A;
                    mappedAnalysis.HDI_T = report.HDI_T;
                    mappedAnalysis.FAI_C7 = report.FAI_C7;
                    mappedAnalysis.FAI_A = report.FAI_A;
                    mappedAnalysis.FAI_T = report.FAI_T;
                    

                    // Store keypoints
                    mappedAnalysis.Keypoints = new List<Keypoint>
                    {
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.C7.Name, X = report.C7.X, Y = report.C7.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.T7.Name, X = report.T7.X, Y = report.T7.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.LeftHip.Name, X = report.LeftHip.X, Y = report.LeftHip.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.RightHip.Name, X = report.RightHip.X, Y = report.RightHip.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.MidHip.Name, X = report.MidHip.X, Y = report.MidHip.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.LeftScapula.Name, X = report.LeftScapula.X, Y = report.LeftScapula.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.RightScapula.Name, X = report.RightScapula.X, Y = report.RightScapula.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.LeftShoulder.Name, X = report.LeftShoulder.X, Y = report.LeftShoulder.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.RightShoulder.Name, X = report.RightShoulder.X, Y = report.RightShoulder.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.LeftSide.Name, X = report.LeftSide.X, Y = report.LeftSide.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.RightSide.Name, X = report.RightSide.X, Y = report.RightSide.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.LeftUnderArm.Name, X = report.LeftUnderArm.X, Y = report.LeftUnderArm.Y },
                        new Keypoint { AnalysisId = mappedAnalysis.Id, Name = report.RightUnderArm.Name, X = report.RightUnderArm.X, Y = report.RightUnderArm.Y }
                    };

                    _unitOfWork.AnalysisRepositry.Update(mappedAnalysis);
                    _unitOfWork.Complete();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Processing failed: {ex.Message}");
                    return View(analysisVm);
                }

                return RedirectToAction(nameof(Index), new { patientId = analysisVm.PatientId });
            }
            return View(analysisVm);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            var analysis = _unitOfWork.AnalysisRepositry.GetAll()
                 
                .FirstOrDefault(e => e.Id == id.Value);
            if (analysis == null)
                return NotFound();
            var mappedAnalysis = _mapper.Map<Analysis, AnalysisViewModel>(analysis);
            return View(viewName, mappedAnalysis);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, AnalysisViewModel analysisVm)
        {
            var analysis = _unitOfWork.AnalysisRepositry.GetAll().FirstOrDefault(e => e.Id == id);
            if (id != analysisVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedAnalysis = _mapper.Map<AnalysisViewModel, Analysis>(analysisVm);
                    _unitOfWork.AnalysisRepositry.Update(mappedAnalysis);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index), new { patientId = analysis.PatientId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(analysisVm);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete([FromRoute] int id, AnalysisViewModel analysisVm)
        {
            var analysis = _unitOfWork.AnalysisRepositry.GetAll().FirstOrDefault(e => e.Id == id);
            if (id != analysisVm.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedAnalysis = _mapper.Map<AnalysisViewModel, Analysis>(analysisVm);
                    _unitOfWork.AnalysisRepositry.Delete(mappedAnalysis);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index), new { patientId = analysis.PatientId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(analysisVm);
        }

        [HttpPost]
        public IActionResult AddNote(AnalysisViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var analysis = _unitOfWork.AnalysisRepositry.GetById(viewModel.Id);
                if (analysis == null)
                    return NotFound();

                var note = new Note
                {
                    Content = viewModel.NewNote,
                    AnalysisId = viewModel.Id
                };

                analysis.Notes.Add(note);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction("Details", new { id = viewModel.Id });
            }
            return View("Details", viewModel);
        }
    }
}