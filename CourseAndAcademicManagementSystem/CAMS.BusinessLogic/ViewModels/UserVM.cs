using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Remote(action: "VerifyEmail", controller: "Users")]
        public string Email { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!; 
    }
}
