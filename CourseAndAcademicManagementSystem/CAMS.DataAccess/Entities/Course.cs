using CAMS.DataAccess.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Entities
{
    public class Course :BaseEntity<int>
    {

        public string? Code { get; set; } 

        public string Name { get; set; } = null!;


        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public bool IsActive { get; set; }

        public int? Credits { get; set; }

        public int? DurationHours { get; set; }

        public int? MaxTrainees { get; set; }

        public string? ThumbnailUrl { get; set; }


        public int? InstructorId { get; set; }

        public User? Instructor { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }



    }
}
