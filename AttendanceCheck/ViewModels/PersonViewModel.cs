using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.ViewModels
{
    public class PersonViewModel
    {
		public int GroupId { get; set; }

		//[Key]
		public int PersonId { get; set; }

		//[Required]
		//[Display(Name = "Last Name")]
		//[MaxLength(70)]
		public string LastName { get; set; }

		//[Required]
		//[Display(Name = "First Name")]
		//[MaxLength(70)]
		public string FirstName { get; set; }

		//[Required]
		//[Display(Name = "Contact No.")]
		//[MaxLength(22)]
		public string ContactNo { get; set; }
    }

	public class CreatePersonViewModel
	{
		public int GroupId { get; set; }

		[Required]
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

	//public class EditPersonViewModel
	//{
	//	public int GroupId { get; set; }

	//	public int PersonId { get; set; }

	//	[Required]
	//	[Display(Name = "Last Name")]
	//	[MaxLength(70)]
	//	public string LastName { get; set; }

	//	[Required]
	//	[Display(Name = "First Name")]
	//	[MaxLength(70)]
	//	public string FirstName { get; set; }

	//	[Required]
	//	[Display(Name = "Contact No.")]
	//	[MaxLength(22)]
	//	public string ContactNo { get; set; }
	//}

	public class PersonGroupViewModel
    {
        public int GroupId { get; set; }
        public int PersonId { get; set; }
        public string FullName { get; set; }
    }

	public class PGVM
    {
        public IEnumerable<GroupViewModel> GroupVM { get; set; }
        public EditPersonViewModel EditPersonVM { get; set; }
    }
}
