using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly EmployeeManagementContext _context;

        public FormRepository(EmployeeManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Form>> GetAllAsync()
        {
            return await _context.Form.ToListAsync();
        }

        public async Task<Form> GetByIdAsync(int id)
        {
            return await _context.Form.FindAsync(id);
        }

        public async Task AddAsync(Form form)
        {
            await _context.Form.AddAsync(form);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Form form)
        {
            _context.Form.Update(form);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var form = await _context.Form.FindAsync(id);
            if (form != null)
            {
                _context.Form.Remove(form);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Form>> GetFormByUserIdAsync(string userId)
        {
            return await _context.Form.Where(f => f.UserId == userId).ToListAsync();
        }
    }
}
