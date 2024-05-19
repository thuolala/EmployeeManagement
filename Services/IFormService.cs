using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface IFormService
    {
        Task<IEnumerable<Form>> GetAllFormsAsync();
        Task<Form> GetFormByIdAsync(int id);
        Task AddFormAsync(Form form);
        Task UpdateFormAsync(Form form);
        Task DeleteFormAsync(int id);
        Task<IEnumerable<Form>> GetFormByUserIdAsync(string userId);
    }
}
