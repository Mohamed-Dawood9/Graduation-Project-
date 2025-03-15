using GP.BLL.Interfaces;
using GP.DAL.Data.Models;
using GP.PL.VIewModel;
using Microsoft.AspNetCore.Mvc;

namespace GP.PL.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public IActionResult Index()
        //{
        //    var appointment = _unitOfWork.AppointmentsRepositry.GetAll();

        //    var mappedpatient = _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientViewModel>>(patients);
        //    return View(mappedpatient);
        //}
    }
}
