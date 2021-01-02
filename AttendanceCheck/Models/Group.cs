using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.Models
{
    public class Group
    {
		public ApplicationUser AppUser { get; set; }

		[Key]
		public int GroupId { get; set; }

		[Required]
		[MaxLength(80)]
		[Display(Name = "Group Name")]
		public string GroupName { get; set; }

        public List<Person> People { get; set; }
    }
}