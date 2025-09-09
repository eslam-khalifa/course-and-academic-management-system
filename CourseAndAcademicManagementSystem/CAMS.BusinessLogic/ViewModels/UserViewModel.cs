using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailUnique", controller: "Users", AdditionalFields = "UserId")]
        public string Email { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [StringLength(250)]
        public string? ProfilePictureUrl { get; set; }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
