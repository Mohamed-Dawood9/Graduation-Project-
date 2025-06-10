using AutoMapper;
using GP.BLL.Interfaces;
using GP.DAL.Data.Models;
using GP.PL.VIewModel;
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

        public AnalysisController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var analysis = _unitOfWork.AnalysisRepositry.GetById(id.Value);
            if (analysis == null)
                return NotFound();
            var mappedAnalysis = _mapper.Map<Analysis, AnalysisViewModel>(analysis);
            return View("Details", mappedAnalysis);
        }

        [HttpPost]
        public IActionResult AddNote(AnalysisViewModel viewModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(viewModel.NewNote))
            {
                var analysis = _unitOfWork.AnalysisRepositry.GetById(viewModel.Id);
                if (analysis == null)
                    return NotFound();

                var note = new Note
                {
                    Content = viewModel.NewNote,
                    AnalysisId = viewModel.Id
                };

                _unitOfWork.NotesRepositry.Add(note);
                _unitOfWork.Complete();
                return RedirectToAction("Details", new { id = viewModel.Id });
            }

            var existingAnalysis = _unitOfWork.AnalysisRepositry.GetById(viewModel.Id);
            if (existingAnalysis == null)
                return NotFound();
            var mappedAnalysis = _mapper.Map<Analysis, AnalysisViewModel>(existingAnalysis);
            return View("Details", mappedAnalysis);
        }

        [HttpPost]
        public IActionResult DeleteNote(int noteId, int analysisId)
        {
            var note = _unitOfWork.NotesRepositry.GetById(noteId);
            if (note == null || note.AnalysisId != analysisId)
                return NotFound();

            _unitOfWork.NotesRepositry.Remove(note);
            _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = analysisId });
        }
    }
}