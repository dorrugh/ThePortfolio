using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThePortfolio.Models;

namespace ThePortfolio.Data
{
    public class ProjectContext: DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) :
             base(options)
        {

        }
        public DbSet<Project> ProjectTbl { get; set; }// table
    }
}
