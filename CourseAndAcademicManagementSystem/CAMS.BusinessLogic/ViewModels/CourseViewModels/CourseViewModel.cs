using CAMS.BusinessLogic.Services.Validators;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.CourseViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [NoNumber]
        [Remote(action: "IsCourseNameUnique", controller: "Courses", AdditionalFields = "CourseId")]
        public string Name { get; set; } = null!;

        [StringLength(20)]
        [Remote(action: "IsCourseCodeUnique", controller: "Courses", AdditionalFields = "CourseId")]
        public string? Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = null!;

        public int? InstructorId { get; set; }

        public string? InstructorName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
