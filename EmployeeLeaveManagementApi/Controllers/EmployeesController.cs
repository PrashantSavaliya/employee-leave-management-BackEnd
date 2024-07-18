using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagementApi.Data;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeLeaveManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public EmployeesController(EmployeeDbContext context)
        {
            _context = context;
        }

        [HttpGet("Getall")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllActiveEmployees()
        {
            var employees = await _context.Employees.Where(e => e.IsActive).ToListAsync();
            if (employees == null || employees.Count == 0)
            {
                return NotFound(new { message = "No active employees found." });
            }

            return Ok(new { message = "Active employees retrieved successfully.", data = employees });
        }

        [HttpGet("Getbyid")]
        public async Task<ActionResult<Employee>> GetEmployeeById()
        {
            Guid id = Guid.Parse(User.Identity.Name);
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || !employee.IsActive)
            {
                return NotFound(new { message = "Employee not found or inactive." });
            }

            return Ok(new { message = "Employee retrieved successfully.", data = employee });
        }

        [HttpPut("Update")]
        public async Task<ActionResult<Employee>> UpdateEmployee(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);

            if (existingEmployee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }
            employee.IsActive = true;
            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw; 
            }
            return Ok(new { message = "Employee updated successfully.", data = existingEmployee });
        }
        // This is delete method
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || !employee.IsActive)
            {
                return NotFound(new { message = "Employee not found or already inactive." });
            }

            // Soft delete: Mark the employee as inactive
            employee.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee Deleted successfully.", data = id });
        }
    }
}
