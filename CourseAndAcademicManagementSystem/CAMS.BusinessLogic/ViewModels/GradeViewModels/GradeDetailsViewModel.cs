using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.GradeViewModels
{
    public class GradeDetailsViewModel
    {
        public int GradeId { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public int TraineeId { get; set; }
        public UserVM? Trainee { get; set; } = null!;
        public int Value { get; set; }
        public decimal? Weight { get; set; }
        public int AttemptNumber { get; set; } = 1;
        public bool? IsFinal { get; set; }
        public DateTime? GradedAt { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
