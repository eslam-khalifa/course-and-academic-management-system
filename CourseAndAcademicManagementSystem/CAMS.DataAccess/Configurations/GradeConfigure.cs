using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class GradeConfigure : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            
           
            builder.HasOne(s => s.Session).WithMany().HasForeignKey(s => s.SessionId);
            builder.HasOne(t => t.Trainee).WithMany().HasForeignKey(t => t.TraineeId);

            builder.Property(g => g.Value)
                   .IsRequired().HasMaxLength(100);
            builder.Property(g => g.Weight).HasColumnType("decimal(5,2)");

            builder.Property(g => g.AttemptNumber)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(g=>g.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.Property(g => g.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(g => g.Comments)
                   .HasMaxLength(500);
        }
    }
}
