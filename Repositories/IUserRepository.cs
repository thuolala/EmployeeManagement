using EmployeeManagement.Models;
using EmployeeManagement.Models.DTO;

namespace EmployeeManagement.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllAsync();
        Task<Users> GetByIdAsync(string id);

        Task<Users> GetByEmailAsync(string email);
        Task AddAsync(UsersDTO user);
        Task UpdateAsync(Users Users);
        Task DeleteAsync(string id);
    }
}
