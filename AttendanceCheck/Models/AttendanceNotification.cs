using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.Models
{
    public class AttendanceNotification
    {
		public ApplicationUser AppUser { get; set; }

		public Person PersonInNotif { get; set; }

		[Key]
		public int AttNotificationId { get; set; }

		public DateTime CreatedDate { get; set; }

		public int NotifNoOfAbsentCount { get; set; }

		[Required]
		public bool Noted { get; set; }

		public DateTime NotedDate { get; set; }

		[Required]
		[MaxLength(300)]
		public string ActionMade { get; set; }
	}
}
