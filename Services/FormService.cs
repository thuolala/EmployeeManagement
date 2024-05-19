using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;

        public FormService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public async Task<IEnumerable<Form>> GetAllFormsAsync()
        {
            return await _formRepository.GetAllAsync();
        }

        public async Task<Form> GetFormByIdAsync(int id)
        {
            return await _formRepository.GetByIdAsync(id);
        }

        public async Task AddFormAsync(Form form)
        {
            await _formRepository.AddAsync(form);
        }

        public async Task UpdateFormAsync(Form form)
        {
            await _formRepository.UpdateAsync(form);
        }

        public async Task DeleteFormAsync(int id)
        {
            await _formRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Form>> GetFormByUserIdAsync(string userId)
        {
            return await _formRepository.GetFormByUserIdAsync(userId);
        }
    }
}
