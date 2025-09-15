using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.GradeViewModels
{
    public class GradeViewModel
    {
        public int GradeId { get; set; }

        [Required]
        public int SessionId { get; set; }
        public string? SessionName { get; set; }

        [Required]
        public int TraineeId { get; set; }

        public string? TraineeName { get; set; }

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }

        [Range(0, 100)]
        public decimal? Weight { get; set; }

        public bool IsFinal { get; set; } = false;

        public DateTime? GradedAt { get; set; }
    }
}
