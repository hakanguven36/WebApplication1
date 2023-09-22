using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class MyContext : DbContext
	{
		public MyContext(DbContextOptions<MyContext> options) : base(options)
		{
		}
		public DbSet<User> User { get; set; }
		public DbSet<Etiket> Etiket { get; set; }
		public DbSet<Tamamlayan> Tamamlayan { get; set; }
	}
}
