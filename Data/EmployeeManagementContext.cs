using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;

namespace EmployeeManagement.Data
{
    public class EmployeeManagementContext : DbContext
    {

        public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeeManagement.Models.Users> Users { get; set; } = default!;
        public object UsersDTO { get; internal set; }
    }
}
