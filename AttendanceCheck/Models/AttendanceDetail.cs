using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.Models
{
    
    public class AttendanceDetail
    {
        public Attendance Attendance { get; set; }

        public Person PersonAttDetail { get; set; }

        [Key]
        public int AttendanceDetailId { get; set; }

        public AttendanceStatus AttStatus { get; set; }
    }
}