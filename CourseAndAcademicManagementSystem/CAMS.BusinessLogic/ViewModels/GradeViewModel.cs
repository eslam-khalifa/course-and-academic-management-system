using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels
{
    public class GradeViewModel
    {
        public int GradeId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required]
        public int TraineeId { get; set; }

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }

        [Range(0, 100)]
        public decimal? Weight { get; set; }

        [Range(1, int.MaxValue)]
        public int AttemptNumber { get; set; } = 1;

        public bool IsFinal { get; set; } = false;

        public int? GradedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? GradedAt { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }
    }
}
