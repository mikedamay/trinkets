using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ting
{
	public class Getter
	{
		public void Get()
		{
			try
			{
				using (var client = new HttpClient())
				{
					using( var response = client.GetStreamAsync(new Uri(
					  "http://nuget.org/api/v2/Packages(Id='PureDI',Version='0.1.2-alpha')")))
					{
						Stream s = response.Result;
						using (var sr = new StreamReader(s))
						{
							string line = null;
							int ctr = 0;
							while((line = sr.ReadLine()) != null)
							{
								Console.WriteLine(line);
								if (++ctr >= 100)
								{
									break;
								}
							}
						}
					}	
				}
			}
			#pragma warning disable 168
			catch (Exception ex)
			{
				throw;
			}
			#pragma warning restore 160
		}
	}
}
