using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttendanceCheck.ApplicationEFCore;
using AttendanceCheck.Models;
using Microsoft.AspNetCore.Authorization;
using AttendanceCheck.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AttendanceCheck.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly AttendanceCheckDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public GroupController(AttendanceCheckDbContext context,
                               SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Group
        public async Task<IActionResult> Index()
        {
            //var userId = await GetCurrentUserIdAsync();
            //var groupVM = await _context.Groups
            //    .Where(g => g.AppUser.IdentityGUID == userId)
            //    .Select(s => new GroupViewModel { GroupId = s.GroupId, GroupName = s.GroupName })
            //    .ToListAsync();

            return View(await GetGroups());
        }

        [HttpGet]
        public async Task<IActionResult> LoadData()
        {
            return Json(await GetGroups());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateGroupViewModel @group)
        {
            if (ModelState.IsValid)
            {
                var userId = await GetCurrentUserIdAsync();
                var currentAppUser = await _context.ApplicationUsers
                    .Where(u => u.IdentityGUID == userId)
                    .FirstOrDefaultAsync();

                var newGroup = new Group
                {
                    AppUser = currentAppUser,
                    GroupName = @group.GroupName
                };

                _context.Add(newGroup);
                await _context.SaveChangesAsync();
                
                return Ok(new { Message = "Successfully create new group" });
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] EditGroupViewModel @group)
        {

            if (ModelState.IsValid)
            {
                if (group.GroupId == 0 || group.GroupId < 0)
                {
                    return BadRequest(new { Message = "Invalid input(s)" });
                }

                var userId = await GetCurrentUserIdAsync();
                var appUserGroup = await _context.Groups
                    .Where(g => g.AppUser.IdentityGUID == userId && g.GroupId == group.GroupId)
                    .FirstOrDefaultAsync();

                if (appUserGroup == null)
                {
                    return BadRequest(new { Message = "Invalid input(s)" });
                }

                appUserGroup.GroupName = group.GroupName;

                _context.Update(appUserGroup);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Successfully updated group" });
            }

            return BadRequest(new { Message = "Invalid input(s)" });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0 || id < 0)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            var userId = await GetCurrentUserIdAsync();
            //var appUserId = await _context.ApplicationUsers
            //    .Where(a => a.IdentityGUID == userId)
            //    //.Select(s => new { x= s.UserId })
            //    .SingleOrDefaultAsync();
            //var z = appUserId.UserId;
            var appUserGroup = await _context.Groups
                .Include(g => g.AppUser)
                .Where(g => g.GroupId == id && g.AppUser.IdentityGUID == userId)
                .SingleOrDefaultAsync();

            if (appUserGroup == null)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            _context.Groups.Remove(appUserGroup);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully deleted group" });
        }

        private async Task<IEnumerable<GroupViewModel>> GetGroups()
        {
            var userId = await GetCurrentUserIdAsync();
            return await _context.Groups
                .Where(g => g.AppUser.IdentityGUID == userId)
                .Select(s => new GroupViewModel { GroupId = s.GroupId, GroupName = s.GroupName })
                .ToListAsync();
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);

            return user.Id;
        }
    }
}