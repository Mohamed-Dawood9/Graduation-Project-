using AutoMapper;
using GP.BLL.Interfaces;
using GP.DAL.Data.Models;
using GP.PL.Helper;
using GP.PL.VIewModel;
using GP_BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace GP.PL.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISpineProcessingService _spineProcessingService;

        public AppointmentController(IUnitOfWork unitOfWork, IMapper mapper, ISpineProcessingService spineProcessingService)
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

            var appointments = _unitOfWork.AppointmentsRepositry
                   .GetAllWithAnalysis() // Use new method
                   .Where(a => a.PatientId == patientId)
                   .OrderByDescending(a => a.Date)
                   .ToList();

            var mappedApp = _mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentViewModel>>(appointments);

            var viewModel = new PatientAppointmentViewModel
            {
                Patient = patient,
                Appointments = mappedApp
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int patientId, IFormFile photo)
        {
            if (photo == null || (!photo.FileName.EndsWith(".jpg") && !photo.FileName.EndsWith(".jpeg")))
                return Json(new { success = false, message = "Please upload a .jpg or .jpeg file." });

            try
            {
                // Step 1: Upload the photo
                string photoPath = DocumentSettings.UpdloadFile(photo, "images");
                if (string.IsNullOrEmpty(photoPath))
                    return Json(new { success = false, message = "Failed to upload the file." });

                // Create Appointment
                var appointment = new Appointment
                {
                    PatientId = patientId,
                    DoctorId = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst("DoctorId")?.Value ?? "0") : 0,
                    Date = DateTime.Now,
                    OrginalPhotoPath = photoPath
                };

                _unitOfWork.AppointmentsRepositry.Add(appointment);
                int result = _unitOfWork.Complete();
                if (result <= 0)
                    return Json(new { success = false, message = "Failed to save appointment." });

                // Step 2: Create folder structure for 3D processing
                string patientFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "processed_images", $"Patient_{patientId}");
                string appointmentFolder = Path.Combine(patientFolder, $"Appointment_{appointment.Id}");
                Directory.CreateDirectory(appointmentFolder);

                // Step 3: Process the photo for 3D (adapted from .glb processing)
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", photoPath);
                if (!System.IO.File.Exists(imagePath))
                    return Json(new { success = false, message = $"Image file not found at: {imagePath}." });

                // Assuming ProcessSpine can handle .jpg; adjust if a different method is needed
                string stlPath = "E:\\Year 4\\sem2\\gp\\final\\Spine_NIH3D.stl";
                _spineProcessingService.ProcessSpine(imagePath, stlPath, appointmentFolder);

                // Step 4: Get processed HTML files
                string baseFileName = Path.GetFileNameWithoutExtension(photo.FileName);
                var allFiles = Directory.GetFiles(appointmentFolder, "*.html");
                if (allFiles.Length != 3)
                    return Json(new { success = false, message = $"Expected 3 HTML files, found {allFiles.Length}." });

                appointment.ProcessedPhotoPath1 = $"/processed_images/Patient_{patientId}/Appointment_{appointment.Id}/{Path.GetFileName(allFiles[0])}";
                appointment.ProcessedPhotoPath2 = $"/processed_images/Patient_{patientId}/Appointment_{appointment.Id}/{Path.GetFileName(allFiles[1])}";
                appointment.ProcessedPhotoPath3 = $"/processed_images/Patient_{patientId}/Appointment_{appointment.Id}/{Path.GetFileName(allFiles[2])}";

                // Step 5: Read and store the Cobb angle
                var cobbAngleFiles = Directory.GetFiles(appointmentFolder, $"*{baseFileName}_cobb_angle.txt");
                if (cobbAngleFiles.Any())
                {
                    string cobbAngleText = System.IO.File.ReadAllText(cobbAngleFiles.First());
                    appointment.CobbAngle = decimal.TryParse(Regex.Match(cobbAngleText, @"\d+\.\d+").Value, out decimal angle) ? angle : 0m;
                }

                _unitOfWork.AppointmentsRepositry.Update(appointment);
                result = _unitOfWork.Complete();
                if (result <= 0)
                    return Json(new { success = false, message = "Failed to update appointment." });

                // Step 6: Create Analysis for 2D processing
                var analysis = new Analysis
                {
                    PatientId = patientId,
                    DoctorId = appointment.DoctorId,
                    Date = DateTime.Now,
                    OriginalPhotoPath = photoPath,
                    AppointmentId = appointment.Id
                };

                _unitOfWork.AnalysisRepositry.Add(analysis);
                result = _unitOfWork.Complete();
                if (result <= 0)
                    return Json(new { success = false, message = "Failed to save analysis." });

                // Step 7: Create folder structure for 2D processing
                string analysisFolder = Path.Combine(patientFolder, $"Analysis_{analysis.Id}");
                Directory.CreateDirectory(analysisFolder);

                // Step 8: Process the photo for 2D analysis
                var report = await _spineProcessingService.ProcessBackImageAsync(imagePath, analysisFolder);
                string outputImagePath = Path.Combine(analysisFolder, "AnnotatedImage.jpg");
                _spineProcessingService.AnnotateAndSaveImage(report, imagePath, outputImagePath);

                analysis.ProcessedPhotoPath = $"/processed_images/Patient_{patientId}/Analysis_{analysis.Id}/AnnotatedImage.jpg";
                analysis.HDI_S = report.HDI_S;
                analysis.HDI_A = report.HDI_A;
                analysis.HDI_T = report.HDI_T;
                analysis.FAI_C7 = report.FAI_C7;
                analysis.FAI_A = report.FAI_A;
                analysis.FAI_T = report.FAI_T;

                analysis.Keypoints = new List<Keypoint>
                {
                    new Keypoint { AnalysisId = analysis.Id, Name = report.C7.Name, X = report.C7.X, Y = report.C7.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.T7.Name, X = report.T7.X, Y = report.T7.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.LeftHip.Name, X = report.LeftHip.X, Y = report.LeftHip.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.RightHip.Name, X = report.RightHip.X, Y = report.RightHip.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.MidHip.Name, X = report.MidHip.X, Y = report.MidHip.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.LeftScapula.Name, X = report.LeftScapula.X, Y = report.LeftScapula.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.RightScapula.Name, X = report.RightScapula.X, Y = report.RightScapula.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.LeftShoulder.Name, X = report.LeftShoulder.X, Y = report.LeftShoulder.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.RightShoulder.Name, X = report.RightShoulder.X, Y = report.RightShoulder.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.LeftSide.Name, X = report.LeftSide.X, Y = report.LeftSide.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.RightSide.Name, X = report.RightSide.X, Y = report.RightSide.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.LeftUnderArm.Name, X = report.LeftUnderArm.X, Y = report.LeftUnderArm.Y },
                    new Keypoint { AnalysisId = analysis.Id, Name = report.RightUnderArm.Name, X = report.RightUnderArm.X, Y = report.RightUnderArm.Y }
                };

                _unitOfWork.AnalysisRepositry.Update(analysis);
                result = _unitOfWork.Complete();
                if (result <= 0)
                    return Json(new { success = false, message = "Failed to update analysis." });

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create error: {ex.Message}");
                return Json(new { success = false, message = $"Processing failed: {ex.Message}" });
            }
        }

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var appointment = _unitOfWork.AppointmentsRepositry.GetById(id.Value);
            if (appointment == null)
                return NotFound();
            var mappedApp = _mapper.Map<Appointment, AppointmentViewModel>(appointment);
            return View("Details", mappedApp);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var appointment = _unitOfWork.AppointmentsRepositry.GetById(id);
            if (appointment == null)
                return Json(new { success = false, message = "Appointment not found." });

            var analysis = _unitOfWork.AnalysisRepositry.GetAll().FirstOrDefault(a => a.AppointmentId == id);
            if (analysis != null)
            {
                _unitOfWork.AnalysisRepositry.Delete(analysis);
            }

            _unitOfWork.AppointmentsRepositry.Delete(appointment);
            _unitOfWork.Complete();

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AddNote(AppointmentViewModel viewModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(viewModel.NewNote))
            {
                var appointment = _unitOfWork.AppointmentsRepositry.GetById(viewModel.Id);
                if (appointment == null)
                    return NotFound();

                var note = new Note
                {
                    Content = viewModel.NewNote,
                    AppointmentId = viewModel.Id
                };

                _unitOfWork.NotesRepositry.Add(note);
                _unitOfWork.Complete();
                return RedirectToAction("Details", new { id = viewModel.Id });
            }

            var existingAppointment = _unitOfWork.AppointmentsRepositry.GetById(viewModel.Id);
            if (existingAppointment == null)
                return NotFound();
            var mappedApp = _mapper.Map<Appointment, AppointmentViewModel>(existingAppointment);
            return View("Details", mappedApp);
        }

        [HttpPost]
        public IActionResult DeleteNote(int noteId, int appointmentId)
        {
            var note = _unitOfWork.NotesRepositry.GetById(noteId);
            if (note == null || note.AppointmentId != appointmentId)
                return NotFound();

            _unitOfWork.NotesRepositry.Remove(note);
            _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = appointmentId });
        }
    }
}