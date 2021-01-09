using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttendanceCheck.ApplicationEFCore;
using AttendanceCheck.Models;
using Microsoft.AspNetCore.Identity;
using AttendanceCheck.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceCheck.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly AttendanceCheckDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public PersonController(AttendanceCheckDbContext context,
                               SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Person
        [HttpGet]
        public async Task<IActionResult> Index()
        {


            //return View(await _context.People.ToListAsync());
            return View(await GetGroups());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadPersonGroupData([FromBody] int id)
        {
            if (id == 0 || id < 0)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            var currentUserId = await GetCurrentUserIdAsync();

            var isValidGroupByCurrentUser = await _context.Groups
                .Include(a => a.AppUser)
                .Include(p=>p.People)
                .Where(g => g.AppUser.IdentityGUID == currentUserId
                        && g.GroupId == id
                        && g.People.Where(x => x.PersonGroup.GroupId == id).Count() > 0)
                .ToListAsync() != null;

            if (!isValidGroupByCurrentUser)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            var people = await _context.People
                .Where(p => p.PersonGroup.GroupId == id)
                .Select(g => new PersonViewModel
                {
                    GroupId = g.PersonGroup.GroupId,
                    PersonId = g.PersonId,
                    LastName = g.LastName,
                    FirstName = g.FirstName,
                    ContactNo = g.ContactNo
                })
                .ToListAsync();

            //return View(await _context.People.ToListAsync());
            return Json(people);
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,LastName,FirstName,ContactNo")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,LastName,FirstName,ContactNo")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.PersonId == id);
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
