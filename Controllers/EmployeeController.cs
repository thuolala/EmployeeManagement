using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Authorize(Policy = "Employee")]
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFormService _formService;
        private readonly ISalaryService _salaryService;

        public EmployeeController(IUserService userService, IFormService formService, ISalaryService salaryService)
        {
            _userService = userService;
            _formService = formService;
            _salaryService = salaryService;
        }

        // GET: Info
        [HttpGet("/MyInfo")]
        public async Task<ActionResult<Users>> GetInfo()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email not found in token.");
            }

            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: Form
        [HttpPost("/UploadForm")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadForm([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected");
            }

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var currentUser = await _userService.GetUserByEmailAsync(userEmail);

            byte[] formContent;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                formContent = memoryStream.ToArray();
            }

            var form = new Form
            {
                UserId = currentUser.Id,
                DateCreated = DateTime.UtcNow,
                FormType = Path.GetExtension(file.FileName).ToLowerInvariant(),
                FormName = Path.GetFileNameWithoutExtension(file.FileName),
                FormContent = formContent
            };

            await _formService.AddFormAsync(form);

            return Ok();
        }

        // GET: Salary 
        [HttpGet("/MySalary")]
        public async Task<ActionResult<Salary>> GetSalary()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email not found in token.");
            }

            var currentUser = await _userService.GetUserByEmailAsync(email);
            var salary = await _salaryService.GetSalaryByUserIdAsync(currentUser.Id);
            if (salary == null)
            {
                return NotFound();
            }

            return Ok(salary);
        }

    }
}
