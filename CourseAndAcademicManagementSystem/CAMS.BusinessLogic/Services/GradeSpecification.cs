using DataAccessLayer;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services
{
    public class GradeSpecification :BaseSpecification<Grade, int>
    {
        public GradeSpecification(string search,int? sessionId , int? traineeId ,
            int pageNumber = 1, int pageSize = 10)
            : base(p => (p.SessionId == sessionId || !(sessionId != null)) &&
                       (p.TraineeId == traineeId || !(traineeId != null)))
        {
            switch (search)
            {
                case "asc":
                    AddOrderBy(p => p);
                    break;
                case "desc":
                    AddOrderByDescending(p => p.Value);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            ApplyPaging(pageSize, pageNumber);
            AddInclude(g => g.Trainee);
            AddInclude(g => g.Session);
        }
    }
}
