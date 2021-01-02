using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.Models
{
    public class Attendance
    {
		public ApplicationUser AppUser { get; set; }

		[Key]
		public int AttendanceId { get; set; }

		[Required]
		[Display(Name = "Date")]
		public DateTime AttDate { get; set; }

		public List<AttendanceDetail> AttendanceDetails { get; set; }
	}
}
