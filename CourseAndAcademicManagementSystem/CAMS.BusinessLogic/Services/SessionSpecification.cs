using DataAccessLayer;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services
{
    public class SessionSpecification:BaseSpecification<Session,int>
    {
        public SessionSpecification(int? CourseId,string? search , int pageNumber = 1, int pageSize = 5)
            : base(p => p.Status != "Cancelled"&&(!CourseId.HasValue)||(p.CourseId==CourseId))
        {
            switch (search)
            {
                case "asc":
                    AddOrderBy(p => p.Course.Name);
                    break;
                case "desc":
                    AddOrderByDescending(p => p.Course.Name);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            ApplyPaging(pageSize, pageNumber);
            AddInclude(s => s.Course);
            AddInclude(s => s.Instructor);
        }
    }
}
