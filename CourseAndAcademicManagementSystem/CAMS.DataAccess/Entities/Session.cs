using CAMS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Entities
{
    public class Session :BaseEntity<int>
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }=null!;

        public string? Title { get; set; }

        public string? SessionCode { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime? EnrollmentStartDate { get; set; }

        public DateTime? EnrollmentEndDate { get; set; }

        public string? Location { get; set; }

        public int? Capacity { get; set; }

        public int InstructorId { get; set; }

        public User? Instructor { get; set; } 

        public string Status { get; set; } = null!;

        public string? Mode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
