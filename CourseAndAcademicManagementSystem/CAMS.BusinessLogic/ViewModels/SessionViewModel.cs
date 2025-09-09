using CAMS.BusinessLogic.Services.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels
{
    public class SessionViewModel
    {
        public int SessionId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [EndDateAfter("StartDate", ErrorMessage = "End Date must be after Start Date")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EnrollmentStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EnrollmentEndDate { get; set; }

        public int? InstructorId { get; set; }

        [Range(0, int.MaxValue)]
        public int? Capacity { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [StringLength(20)]
        public string? Mode { get; set; }
    }
}
