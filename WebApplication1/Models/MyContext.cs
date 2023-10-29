using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Models
{
	public class MyContext : DbContext
	{
		//string dbPath = "";
		//MyContext( IWebHostEnvironment environment)
  //      {

		//	dbPath = System.IO.Path.Combine(environment.WebRootPath , @"Database\data.db");
		//	//string specialRoot = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
		//	//dbPath = System.IO.Path.Combine(specialRoot , @"Database\data.db");
  //      }

		protected override void OnConfiguring(DbContextOptionsBuilder options) =>
			options.UseSqlite(@"Data Source=Database\data.db");
			
		public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }
		public DbSet<Annotation> Annotation { get; set; }
		public DbSet<Photo> Photo { get; set; }
	}
}
