using AttendanceCheck.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceCheck.ApplicationEFCore
{
    public class AttendanceCheckDbContext : DbContext
    {
        public AttendanceCheckDbContext(DbContextOptions<AttendanceCheckDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceDetail> AttendanceDetails { get; set; }
        public DbSet<AttendanceNotification> AttendanceNotifications { get; set; }
    }
}
