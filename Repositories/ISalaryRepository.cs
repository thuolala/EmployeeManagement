using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories
{
    public interface ISalaryRepository
    {
        Task<IEnumerable<Salary>> GetAllAsync();
        Task<Salary> GetSalaryByIdAsync(int id);
        Task<IEnumerable<Salary>> GetSalaryByUserIdAsync(string userId);
        Task AddSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryAsync(int id);
    }
}
