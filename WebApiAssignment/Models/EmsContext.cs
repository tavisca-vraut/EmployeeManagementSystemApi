using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAssignment.Models
{
    public class EmsContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Managers> Managers { get; set; }

        public EmsContext(DbContextOptions<EmsContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=EmployeeManagementSystem.db");
        }
    }
}
