using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Configurations
{
    public class CourseConfigure : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {

            builder.HasKey(c=>c.Id);
            builder.Property(c=>c.Code).HasColumnType("nvarchar(20)");
            builder.Property(c=>c.Name).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50);
            builder.Property(c=>c.Description).HasColumnType("nvarchar(250)");
            builder.Property(c=>c.Category).HasColumnType("nvarchar(50)");
            builder.Property(c=>c.ThumbnailUrl).HasColumnType("nvarchar(250)");

            builder.Property(c=>c.Credits).HasMaxLength(10);

            builder.HasOne(c=>c.Instructor).WithMany().HasForeignKey(c=>c.InstructorId);

            builder.Property(c=>c.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(c=>c.UpdatedAt).HasDefaultValueSql("getdate()");
            builder.Property(c=>c.IsDeleted).HasDefaultValue(false);

        }
    }
}
