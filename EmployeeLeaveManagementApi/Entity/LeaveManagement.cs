using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;

namespace EmployeeLeaveManagementApi.Entity
{
    public class LeaveManagement
    {
        [Key]
        public Guid? LeaveId { get; set; } // Primary key

        [Required]
        [ForeignKey("Employee")] 
        public Guid EmployeeId { get; set; }
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        public string? Reason { get; set; }

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending; 
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
