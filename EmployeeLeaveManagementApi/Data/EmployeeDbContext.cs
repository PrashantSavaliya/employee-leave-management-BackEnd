using EmployeeLeaveManagementApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementApi.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveManagement> LeaveManagements { get; set; }
    }
}
