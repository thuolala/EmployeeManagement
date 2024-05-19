using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryService(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        public async Task<IEnumerable<Salary>> GetAllSalariesAsync()
        {
            return await _salaryRepository.GetAllAsync();
        }

        public Task<Salary> GetSalaryByIdAsync(int id)
        {
            return _salaryRepository.GetSalaryByIdAsync(id);
        }

        public Task<IEnumerable<Salary>> GetSalaryByUserIdAsync(string userId)
        {
            return _salaryRepository.GetSalaryByUserIdAsync(userId);
        }

        public Task AddSalaryAsync(Salary salary)
        {
            return _salaryRepository.AddSalaryAsync(salary);
        }

        public Task UpdateSalaryAsync(Salary salary)
        {
            return _salaryRepository.UpdateSalaryAsync(salary);
        }

        public Task DeleteSalaryAsync(int id)
        {
            return _salaryRepository.DeleteSalaryAsync(id);
        }
    }
}
