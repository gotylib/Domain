using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Base
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Domain> Domains { get; set; } = null!;

        public ApplicationContext() 
        {

            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Domains;Username=postgres;Password=1234");
        }
    }
}
