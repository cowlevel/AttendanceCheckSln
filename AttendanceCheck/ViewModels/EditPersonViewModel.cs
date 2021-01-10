using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.ViewModels
{
    public class EditPersonViewModel
    {
		public int GroupId { get; set; }

		public int PersonId { get; set; }

		[Required(ErrorMessage = "L Name IS REQUIRED!")]
		[Display(Name = "Last Name")]
		[MaxLength(70)]
		public string LastName { get; set; }

		[Required]
		[Display(Name = "First Name")]
		[MaxLength(70)]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Contact No.")]
		[MaxLength(22)]
		public string ContactNo { get; set; }
	}
}
