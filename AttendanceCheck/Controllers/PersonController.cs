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
            //var pgvm = new PGVM();
            //pgvm.GroupVM = await GetGroups();
            //return View(pgvm);

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
            var groupByCurrentUser = await _context.Groups
                .Where(u => u.AppUser.IdentityGUID == currentUserId
                            && u.GroupId == id)
                .FirstOrDefaultAsync() != null;

            if (groupByCurrentUser == false)
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

        //public async Task<IActionResult> GetPerson(int? id, int? gid)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userIdGUID = await GetCurrentUserIdAsync();
        //    var personOfCurrentUser = await _context.People
        //        .Where(g => g.PersonGroup.AppUser.IdentityGUID == userIdGUID
        //                 && g.PersonGroup.GroupId == gid
        //                 && g.PersonId == id)
        //        .SingleOrDefaultAsync();

        //    var person = await _context.People
        //        .FirstOrDefaultAsync(m => m.PersonId == id);
        //    if (person == null)
        //    {
        //        return NotFound();
        //    }

        //    return PartialView("/Views/Person/_ModalPersonPartial", personOfCurrentUser);
        //}

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreatePersonViewModel person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            var userIdGUID = await GetCurrentUserIdAsync();
            var groupOfCurrentUser = await _context.Groups
                .Where(a => a.AppUser.IdentityGUID == userIdGUID
                        && a.GroupId == person.GroupId)
                .FirstOrDefaultAsync();

            if (groupOfCurrentUser == null)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            var newPerson = new Person
            {
                PersonGroup = groupOfCurrentUser,
                LastName = person.LastName,
                FirstName = person.FirstName,
                ContactNo = person.ContactNo
            };
            
            _context.People.Add(newPerson);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully created new person", NewPerson = newPerson });
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] EditPersonViewModel person)
        {
            if (person.PersonId == 0 || person.PersonId < 0 ||
                person.GroupId == 0 || person.GroupId < 0)
            {
                return BadRequest(new {Message = "Invalid input(s)" });
            }

            if (ModelState.IsValid)
            {
                var userIdGUID = await GetCurrentUserIdAsync();
                var personOfCurrentUser = await _context.People
                    .Where(g => g.PersonGroup.AppUser.IdentityGUID == userIdGUID
                             && g.PersonGroup.GroupId == person.GroupId
                             && g.PersonId == person.PersonId)
                    .SingleOrDefaultAsync();

                if (personOfCurrentUser == null)
                {
                    return BadRequest(new { Message = "Invalid input(s)" });
                }

                personOfCurrentUser.LastName = person.LastName;
                personOfCurrentUser.FirstName = person.FirstName;
                personOfCurrentUser.ContactNo = person.ContactNo;

                _context.People.Update(personOfCurrentUser);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Successfully created new person", UpdatedPerson = person });
            }

            return BadRequest(new
            {
                Message = "Invalid input(s)",
                Errors = ModelState.Values.SelectMany(e => e.Errors)
                            .Select(e => e.ErrorMessage)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            //var person = await _context.People.FindAsync(id);
            //_context.People.Remove(person);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            var userIdGUID = await GetCurrentUserIdAsync();

            var personToDelete = await _context.Groups
                .Where(p => p.AppUser.IdentityGUID == userIdGUID
                         && p.People.FirstOrDefault(s => s.PersonId == id) != null)
                .Select(x => x.People.FirstOrDefault(s => s.PersonId == id))
                .FirstOrDefaultAsync();

            //var personToDelete = await _context.People
            //    .Where(p => p.PersonGroup.AppUser.IdentityGUID == userIdGUID
            //             && p.PersonId == id)
            //    .SingleOrDefaultAsync();

            if (personToDelete == null)
            {
                return BadRequest(new { Message = "Invalid input(s)" });
            }

            _context.People.Remove(personToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Successfully deleted person" });
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
