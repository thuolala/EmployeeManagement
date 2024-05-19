using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories
{
    public interface IFormRepository
    {
        Task<IEnumerable<Form>> GetAllAsync();
        Task<Form> GetByIdAsync(int id);
        Task AddAsync(Form form);
        Task UpdateAsync(Form form);
        Task DeleteAsync(int id);
        Task<IEnumerable<Form>> GetFormByUserIdAsync(string userId);
    }
}
