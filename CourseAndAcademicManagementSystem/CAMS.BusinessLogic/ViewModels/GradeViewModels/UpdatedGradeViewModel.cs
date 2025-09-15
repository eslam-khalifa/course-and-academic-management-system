using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.GradeViewModels
{
    public class UpdatedGradeViewModel
    {
        public int GradeId { get; set; }

        [Required]
        public int SessionId { get; set; }
        public IEnumerable<SelectListItem>? Sessions { get; set; }

        [Required]
        public int TraineeId { get; set; }

        public IEnumerable<SelectListItem>? Trainees { get; set; }

        [Required]
        public int Value { get; set; }

        public decimal? Weight { get; set; }

        public int AttemptNumber { get; set; } = 1;

        public bool? IsFinal { get; set; }

        public string? Comments { get; set; }
    }
}
