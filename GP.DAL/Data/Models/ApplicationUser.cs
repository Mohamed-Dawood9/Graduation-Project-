using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Fname { get; set; }
		public string Lname { get; set; }
		public bool IsAgree { get; set; }
        public string? ProfilePhoto { get; set; }
    }
}
