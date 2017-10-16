namespace Much
{
	using System;
	using PureDI;

	[Bean]
	public class Program
	{
		[BeanReference] private Much much = null;
		public static void Main()
		{
			var pdi = new PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			prog.much.DoAdo();
		}
	}
}
namespace Much
{
	using System;
	using PureDI;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;

	[Bean]
	internal class Much
	{
		public void DoAdo()
		{
			IDbConnection conn = null;
			try
			{
				conn = new SqlConnection("Server=localhost;Database=Notes;User Id=sa;Password=M1cromus");
				conn.Open();
				IDbCommand cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "select * from Note";
				IDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					Console.WriteLine($"{((IDataRecord)rdr)[0]} {((IDataRecord)rdr)[1]}");
				}
			}
			finally
			{
				conn?.Close();
			}
		}
	}
}
