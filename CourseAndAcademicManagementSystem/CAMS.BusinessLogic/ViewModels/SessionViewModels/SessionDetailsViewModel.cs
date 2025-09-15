using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.SessionViewModels
{
    public class SessionDetailsViewModel
    {
        public int? SessionId { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; } = null!;

        public string? Title { get; set; }

        public string? SessionCode { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime? EnrollmentStartDate { get; set; }

        public DateTime? EnrollmentEndDate { get; set; }

        public string? Location { get; set; }

        public int? Capacity { get; set; }

        public int? InstructorId { get; set; }


        public string Status { get; set; } = null!;

        public string? Mode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
