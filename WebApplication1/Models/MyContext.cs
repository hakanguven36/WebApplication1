using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;


namespace WebApplication1.Models
{
	public class MyContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			//ConfigurationManager.ConnectionStrings[0].ConnectionString
			options.UseSqlite("Data Source=Database\\data.db");
		}

		public DbSet<User> User { get; set; }
		public DbSet<Etiket> Etiket { get; set; }
		public DbSet<HamResim> HamResim { get; set; }
		public DbSet<UserSetting> UserSetting { get; set; }
        public DbSet<Proje> Proje { get; set; }

    }
}
