using DataAccessLayer;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services
{
    public class CourseSpecification : BaseSpecification<Course, int>
    {
        public CourseSpecification(string? search = null, int pageNumber = 1, int pageSize = 5)
            : base(p=>p.InstructorId!=null)
        {
            switch (search)
            {
                case "asc":
                    AddOrderBy(p => p.Name);
                    break;
                case "desc":
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }
            ApplyPaging(PageSize, PageIndex);
            AddInclude(c => c.Instructor);
           
        }
    }
}
