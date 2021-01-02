using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.Models
{
    public class ApplicationUser
    {
		[Key]
		public int UserId { get; set; }

		[MaxLength(450)]
		public string IdentityGUID { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		[MaxLength(70)]
		public string LastName { get; set; }

		[Required]
		[Display(Name = "First Name")]
		[MaxLength(70)]
		public string FirstName { get; set; }

		public int NotifNoOfAbsentInMonth { get; set; }
		public List<Group> Groups { get; set; }
	}
}
