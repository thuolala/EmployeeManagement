using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.DTO;
using System.Data.SqlClient;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly EmployeeManagementContext _context;

        public UsersController(EmployeeManagementContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            return _context.Users.ToArray();
        }

        // GET: User/id
        [HttpGet("/User/{id}")]
        public async Task<ActionResult<Users>> GetUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: Users/Create
        [HttpPost("/Create")]
        public async Task<ActionResult<Users>> Create(Users user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorMessages = string.Join("; ", errors);
                Console.WriteLine($"ModelState is invalid: {errorMessages}");
                return BadRequest(ModelState);
            }

            var parameters = new[]
            {
            new SqlParameter("@FullName", user.FullName ?? (object)DBNull.Value),
            new SqlParameter("@DOB", user.DOB),
            new SqlParameter("@Gender", user.Gender ?? (object)DBNull.Value),
            new SqlParameter("@Address", user.Address ?? (object)DBNull.Value),
            new SqlParameter("@Email", user.Email ?? (object)DBNull.Value),
            new SqlParameter("@JoinedDate", user.JoinedDate),
            new SqlParameter("@Phone", user.Phone ?? (object)DBNull.Value),
            new SqlParameter("@IdPos", user.IdPos ?? (object)DBNull.Value),
            new SqlParameter("@IdRole", user.IdRole),
            new SqlParameter("@Password", user.Password ?? (object)DBNull.Value)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.INSERT_USER @FullName, @DOB, @Gender, @Address, @Email, @JoinedDate, @Phone, @IdPos, @IdRole, @Password",
                    parameters);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // DELETE: Users/Delete
        [HttpDelete("/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Users>> DeleteUser(string id)
        {
            id = id.Trim();
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAllUsers));
        }

        /*
        public async Task<IActionResult> Index()
        {
           return View(await _context.User.ToListAsync());
        }

 
        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,DOB,Gender,Address,Email,JoinedDate,Phone,IdPos,IdRole,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,DOB,Gender,Address,Email,JoinedDate,Phone,IdPos,IdRole,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        */
    }
}
