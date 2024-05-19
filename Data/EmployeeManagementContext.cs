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
        public DbSet<EmployeeManagement.Models.Form> Form { get; set; }
        public DbSet<EmployeeManagement.Models.Salary> Salary { get; set; }
        public object UsersDTO { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Form>()
                 .HasKey(f => f.Id);

            /* modelBuilder.Entity<Form>()
                 .HasOne(f => f.User)
                 .WithMany(u => u.Forms)
                 .HasForeignKey(f => f.UserId)
                 .HasConstraintName("FK_UsersId_Files");
            */
            modelBuilder.Entity<Salary>()
                .ToTable(tb => tb.UseSqlOutputClause(false)) ;

            /* modelBuilder.Entity<Salary>()
                 .HasOne(s => s.User)
                 .WithMany(u => u.Salary)
                 .HasForeignKey(s => s.UserId)
                 .HasConstraintName("FK_UsersId_Salary");
            */

            base.OnModelCreating(modelBuilder);
        }
    }
}
