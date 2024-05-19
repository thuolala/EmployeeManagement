using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly EmployeeManagementContext _context;

        public SalaryRepository(EmployeeManagementContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Salary>> GetAllAsync()
        {
            return await _context.Salary.ToListAsync();
        }

        public async Task<Salary> GetSalaryByIdAsync(int id)
        {
            return await _context.Salary.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Salary>> GetSalaryByUserIdAsync(string userId)
        {
            return await _context.Salary.Where(s => s.UserId == userId).ToListAsync();
        }

        public async Task AddSalaryAsync(Salary salary)
        {
            _context.Salary.Add(salary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            _context.Salary.Update(salary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSalaryAsync(int id)
        {
            var salary = await _context.Salary.FindAsync(id);
            if (salary != null)
            {
                _context.Salary.Remove(salary);
                await _context.SaveChangesAsync();
            }
        }
    }
}
