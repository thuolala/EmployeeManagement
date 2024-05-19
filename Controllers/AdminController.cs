using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Services;

namespace EmployeeManagement.Controllers
{
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[controller]")]

    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFormService _formService;
        private readonly ISalaryService _salaryService;

        public AdminController(IUserService userService, IFormService formService, ISalaryService salaryService)
        {
            _userService = userService;
            _formService = formService;
            _salaryService = salaryService;
        }

        //-----CREATE-----//
        [HttpPost("/Create")]
        public async Task<ActionResult<Users>> Create([Bind("FullName, DOB, Gender, Address, Email, JoinedDate, Phone, IdPos, IdRole, Password")] UsersDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userService.AddUserAsync(user);

            return Ok(user);
        }

        //-----READ-----//
        // GET: Users
        [HttpGet("/AllUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: User/id
        [HttpGet("/User/{id}")]
        public async Task<ActionResult<Users>> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid ID");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        //-----UPDATE-----//
        [HttpPut("/Update/{id}")]
        public async Task<ActionResult<Users>> Update(string id, [Bind("Id, FullName,DOB,Gender,Address,Email,JoinedDate,Phone,IdPos,IdRole,Password")] Users user)
        {
            if (!id.Equals(user.Id))
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.UpdateUserAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _userService.GetUserByIdAsync(user.Id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(user);
        }

        //-----DELETE-----//
        // DELETE: Users/Delete
        [HttpDelete("/Delete/{id}")]
        public async Task<ActionResult<Users>> DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid ID");
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return Ok(user);
        }

        //-----FORMS-----//
        // GET:/AllForms
        [HttpGet("/AllForms")]
        public async Task<ActionResult<IEnumerable<Form>>> GetAllForms()
        {
            var forms = await _formService.GetAllFormsAsync();
            return Ok(forms);
        }

        // GET: /Form/{id}
        [HttpGet("/Form/{id}")]
        public async Task<ActionResult<Form>> GetForm(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound();
            }

            return Ok(form);
        }

        // GET: /ByUser/{userId}
        [HttpGet("/FormByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Form>>> GetFormsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid User ID");
            }

            var forms = await _formService.GetFormByUserIdAsync(userId);
            return Ok(forms);
        }

        //-----SALARY-----//
        // GET:/AllSalaries
        [HttpGet("/AllSalaries")]
        public async Task<ActionResult<IEnumerable<Salary>>> GetAllSalaries()
        {
            var salaries = await _salaryService.GetAllSalariesAsync();
            return Ok(salaries);
        }

        // GET: /Salary/{id}
        [HttpGet("/Salary/{id}")]
        public async Task<ActionResult<Salary>> GetSalary(int id)
        {
            var salary = await _salaryService.GetSalaryByIdAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            return Ok(salary);
        }

        // GET: /ByUser/{userId}
        [HttpGet("/SalaryByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Salary>>> GetSalaryByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid User ID");
            }

            var salary = await _salaryService.GetSalaryByUserIdAsync(userId);
            return Ok(salary);
        }

        // POST: AddSalary
        [HttpPost("/AddSalary")]
        public async Task<ActionResult<Salary>> AddSalary(Salary salary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _salaryService.AddSalaryAsync(salary);

            return Ok(salary);
        }

        // PUT: UpdateSlary
        [HttpPut("/UpdateSalary/{uid}")]
        public async Task<ActionResult<Salary>> UpdateSalary(string uid, Salary salary)
        {
            if (!uid.Equals(salary.UserId))
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _salaryService.UpdateSalaryAsync(salary);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _salaryService.GetSalaryByUserIdAsync(salary.UserId) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return Ok(salary);
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
