using GP.DAL.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace GP.PL.VIewModel
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required! ")]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
        [MinLength(3, ErrorMessage = "Min Length of Name is 3 Chars")]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public bool Gender { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
                            , ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }

        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Medical History")]
        public string MedicalHistory { get; set; }
        // One-to-Many with Appointments
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
