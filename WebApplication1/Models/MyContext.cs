using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Models
{
	public class MyContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite("Data Source=Database\\data.db");

		public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }
		public DbSet<Annotation> Annotation { get; set; }
		public DbSet<Photo> Photo { get; set; }
	}
}
