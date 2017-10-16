namespace Big
{
	using System;
	using PureDI;

	[Bean]
	public class Program
	{
		[BeanReference] private Repo repo = null;
		public static void Main()
		{
			var pdi = new PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.AllDiagnosticsToString());
			prog.repo.ReadDb();
		}
	}
}
namespace Big
{
	using System;
	using PureDI;
	using Microsoft.EntityFrameworkCore;
	using System.Linq;

	[Bean]
	internal class Repo
	{
		public void ReadDb()
		{
			var bdc = new BigDbContext();
			var list = bdc.Note.ToList();
			foreach (var note in list)
			{
				Console.WriteLine(note.Payload);
			}
		}
	}
	public class BigDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(
			  "Server=(localdb)\\mssqllocaldb;Database=Notes;Trusted_Connection=True;MultipleActiveResultSets=true"
			//   "Server=localhost;Database=Notes;User Id=sa;Password=M1cromus"

			);
		}
		public DbSet<Note> Note { get; set; }
	}
}
namespace Big
{
    public class Note
    {
        public long Id { get; set; }
        public string Payload { get; set; }

        public Note()
        {
            
        }
        public Note(string payload)
        {
            Payload = payload;
        }
    }
}










