using CAMS.BusinessLogic.ViewModels;
using CAMS.DataAccess.Entities;
using DataAccessLayer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services
{
    public  class UserSpecification : BaseSpecification<User, int>
    {
        public UserSpecification(QueryUser queryUser)
            : base((p => p.Role == queryUser.Role || !(queryUser.Role != null)))
        {
            switch (queryUser.Search)
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
            ApplyPaging(queryUser.PageSize,queryUser.PageIndex);
        }
    }
}
