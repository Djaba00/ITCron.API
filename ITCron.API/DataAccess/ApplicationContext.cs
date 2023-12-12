using System;
using System.Reflection.Emit;
using ITCron.API.DataAccess.Configurations;
using ITCron.API.Models.InternetProtocol;
using Microsoft.EntityFrameworkCore;

namespace ITCron.API.DataAccess
{
	public class ApplicationContext : DbContext
	{
        public DbSet<IPInformation> IPInformation { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IPInformationConfiguration());
        }
    }
}

