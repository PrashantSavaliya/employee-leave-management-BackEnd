using EmployeeLeaveManagementApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Employee; // Added for IConfiguration

namespace EmployeeLeaveManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(EmployeeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Employee>> Register(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid();
           // employee.IsActive = true;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee Register successfully", data = employee });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest model)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == model.Email && e.Password == model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { message = "Login successful", token, user });
        }


        #region Private method
        private string GenerateJwtToken(Employee user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["ConnectionStrings:TokenSecret"].ToString());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, user.EmployeeId.ToString()), // Corrected from Employee.id to user.EmployeeId
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = _configuration["ConnectionStrings:Audience"],
                Issuer = _configuration["ConnectionStrings:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}