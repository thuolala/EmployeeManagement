using EmployeeManagement.Models;
using EmployeeManagement.Models.DTO;

namespace EmployeeManagement.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUserByIdAsync(string id);
        Task<Users> GetUserByEmailAsync(string email);
        Task AddUserAsync(UsersDTO user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(string id);
    }
}
