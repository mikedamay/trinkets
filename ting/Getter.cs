using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using PureDI;
using PureDI.Attributes;

namespace ting
{
	[Bean]
	public class Getter
	{
		[BeanReference] private Bufferer bufferer = null;
		[BeanReference] private ILogger logger = null;
		public void Get()
		{
			try
			{
				string uri =
					  "http://nuget.org/api/v2/Packages()"
					  +"?$orderby=LastUpdated%20desc&$select=Id,Tags"
					  +"&$filter=IsLatestVersion eq true";
				int ctr = 0;
				do
				{
					using (var client = new HttpClient())
					{
						using( var response = client.GetStreamAsync(new Uri(uri)))
						{
							Stream s = response.Result;
							using (var sr = new StreamReader(s))
							{
								uri = bufferer.Buffer(sr);
								if (++ctr >= 10)
								{
									break;
								}
							}
						}	
					}
				} while (uri != null);
			}
			#pragma warning disable 168
			catch (Exception ex)
			{
				throw new Exception("probably a null reference", ex);
			}
			#pragma warning restore 160
		}
	}
}
