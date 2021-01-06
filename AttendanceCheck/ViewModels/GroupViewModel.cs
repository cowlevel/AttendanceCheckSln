using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.ViewModels
{
    public class GroupViewModel
    {
		public int GroupId { get; set; }

		[Required]
		[MaxLength(80)]
		[Display(Name = "Group Name")]
		public string GroupName { get; set; }
    }

	public class CreateGroupViewModel
	{
		[Required]
		[MaxLength(80)]
		[Display(Name = "Group Name")]
		public string GroupName { get; set; }
	}

	public class EditGroupViewModel
    {
		public int GroupId { get; set; }

		[Required]
		[MaxLength(80)]
		[Display(Name = "Group Name")]
		public string GroupName { get; set; }
	}
}