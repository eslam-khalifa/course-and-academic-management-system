using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels
{
    public class QueryUser
    {
        private const int maxPageSize = 5;
        public string? Search { get; set; }
        public string? Role { get; set; }
        public int PageIndex { get; set; }


        private int pagesize;
         public int PageSize 
         { 
          get { return pagesize; } 
          set { pagesize = (value > maxPageSize) ? maxPageSize : value; }
         }




    }
}
