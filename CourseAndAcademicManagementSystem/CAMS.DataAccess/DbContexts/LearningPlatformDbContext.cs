using CAMS.DataAccess.Entities;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DbContexts
{
    public class LearningPlatformDbContext :DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public DbSet<User> Users { get; set; }
        public LearningPlatformDbContext(DbContextOptions<LearningPlatformDbContext> options):base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
         
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
    }
}
