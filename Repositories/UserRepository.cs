using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeManagementContext _context;

        public UserRepository(EmployeeManagementContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UsersDTO user)
        {
            var parameters = new[]
            {
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@DOB", user.DOB),
                new SqlParameter("@Gender", user.Gender),
                new SqlParameter("@Address", user.Address),
                new SqlParameter("@Email", user.Email ),
                new SqlParameter("@JoinedDate", user.JoinedDate),
                new SqlParameter("@Phone", user.Phone),
                new SqlParameter("@IdPos", user.IdPos),
                new SqlParameter("@IdRole", user.IdRole),
                new SqlParameter("@Password", user.Password)
            };

            try
            {
                var result = _context.Database
                 .SqlQueryRaw<Users>("INSERT_USER @FullName, @DOB, @Gender, @Address, @Email, @JoinedDate, @Phone, @IdPos, @IdRole, @Password", parameters)
                 .ToList();

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public async  Task DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Users> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
