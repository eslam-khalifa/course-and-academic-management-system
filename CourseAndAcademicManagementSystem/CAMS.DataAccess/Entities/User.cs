using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.DataAccess.Entities
{
    public  class User:BaseEntity<int>
    {

        public string Name { get; set; } = null!;

        
        public string Email { get; set; } = null!;

       
        public string Role { get; set; } = null!;
    }
}
