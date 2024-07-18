using EmployeeLeaveManagementApi.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Key]
    public Guid? EmployeeId { get; set; } 

    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }

    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
    public int? Age { get; set; }

    [Required(ErrorMessage = "Birthdate is required")]
    public DateTime? Birthdate { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public string? Department { get; set; }

    [Required(ErrorMessage = "Position is required")]
    public string? Position { get; set; }

    public string PreviousLeave { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Please enter password")]
    [MinLength(6, ErrorMessage = "Password must be 6 character long")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Contact Number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string? ContactNumber { get; set; }
    public bool IsActive { get; set; }
}


public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}