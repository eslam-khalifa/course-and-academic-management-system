using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Configurations
{
    public class SessionConfigure : IEntityTypeConfiguration<Session>
    {

        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasOne(c=>c.Course).WithMany().HasForeignKey(c=>c.CourseId);
            builder.Property(c=>c.Title).HasColumnType("nvarchar(100)");

            builder.Property(c=>c.SessionCode).HasColumnType("nvarchar(30)");
            builder.Property(c=>c.Location).HasColumnType("nvarchar(200)");

            builder.HasOne(c => c.Instructor).WithMany().HasForeignKey(c => c.InstructorId);


            builder.Property(c => c.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(c => c.UpdatedAt).HasDefaultValueSql("getdate()");
        }
    }
}
