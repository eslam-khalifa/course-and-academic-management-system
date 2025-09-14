using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DataAccessLayer.Entities
{
    public class Grade: BaseEntity<int>
    {
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public int TraineeId { get; set; }
        public UserApp Trainee { get; set; } = null!;
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
    }
}
