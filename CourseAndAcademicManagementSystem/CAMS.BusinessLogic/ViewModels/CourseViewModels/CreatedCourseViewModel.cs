using CAMS.BusinessLogic.Services.Validators;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.CourseViewModels
{
    public class CreatedCourseViewModel
    {
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

        [StringLength(250)]
        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        [Range(0, 10)]
        public int? Credits { get; set; }

        [Range(0, 1000)]
        public int? DurationHours { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxTrainees { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Instructor is required")]
        public int? InstructorId { get; set; }

        public IEnumerable<SelectListItem>? Instructors { get; set; }

        public string? ThumbnailUrl { get; set; }
    }
}
