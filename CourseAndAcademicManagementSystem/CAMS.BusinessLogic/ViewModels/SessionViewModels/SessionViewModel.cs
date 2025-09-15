using CAMS.BusinessLogic.Services.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        public int SessionId { get; set; }
        public string? SessionCode { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [EndDateAfter("StartDate", ErrorMessage = "End Date must be after Start Date")]
        public DateTime EndDate { get; set; }

        public int? InstructorId { get; set; }
        public string? InstructorName { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [StringLength(20)]
        public string? Mode { get; set; }
    }
}
