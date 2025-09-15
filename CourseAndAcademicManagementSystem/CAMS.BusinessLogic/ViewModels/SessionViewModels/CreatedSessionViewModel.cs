using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.SessionViewModels
{
    public class CreatedSessionViewModel
    {
        public int CourseId { get; set; }

        public IEnumerable<SelectListItem>? Courses { get; set; }

        public string Title { get; set; } = string.Empty;

        public string SessionCode { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? EnrollmentStartDate { get; set; }

        public DateTime? EnrollmentEndDate { get; set; }

        public string? Location { get; set; }

        public int? Capacity { get; set; }

        public int InstructorId { get; set; }

        public IEnumerable<SelectListItem>? Instructors { get; set; }

        public string Status { get; set; } = "Scheduled";

        public string? Mode { get; set; }
    }
}
