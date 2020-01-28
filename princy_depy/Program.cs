namespace PrincyDepy
{
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;

	public class Program
	{
		public static void Main()
		{
		}
	}
	public class Princy
	{
		public long Id { get; set; }
		public List<Depy> Depies { get; set; }
	}
	public class Depy
	{
		public long Id { get; set; }
	}
	public class TheDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder ob)
		{
			ob.UseSqlServer("Server=localhost");
		}
		protected override void OnModelCreating(ModelBuilder mb)
		{
			mb.Entity<Depy>()
				.HasOne<Princy>()
				.WithMany(p => p.Depies);
		}
		public DbSet<Princy> PrincySet { get; set; }
		public DbSet<Depy> DepySet { get; set; }
	}
}

