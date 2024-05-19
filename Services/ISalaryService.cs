using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface ISalaryService
    {
        Task<IEnumerable<Salary>> GetAllSalariesAsync();
        Task<Salary> GetSalaryByIdAsync(int id);
        Task<IEnumerable<Salary>> GetSalaryByUserIdAsync(string userId);
        Task AddSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryAsync(int id);
    }
}
