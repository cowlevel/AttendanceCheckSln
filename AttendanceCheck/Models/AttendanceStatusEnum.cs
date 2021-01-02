using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.Models
{
    public class AttendanceStatusEnum
    {

    }
    public enum AttendanceStatus : int
    {
        Absent = 0,
        Present = 1
    }
}
