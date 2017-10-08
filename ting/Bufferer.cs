using System;
using PureDI;
using System.IO;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace ting
{
	[Bean]
	internal class Bufferer
	{
		[BeanReference] private TagsHandler tagsHandler = null;
		[BeanReference] private ILogger logger = null;
		internal string Buffer(StreamReader reader)
		{
			XPathNavigator nav = new XPathDocument(reader).CreateNavigator();
			var manager = new XmlNamespaceManager(nav.NameTable);
		 	manager.AddNamespace("x","http://www.w3.org/2005/Atom");
			manager.AddNamespace("d","http://schemas.microsoft.com/ado/2007/08/dataservices");
			XPathNodeIterator nodes = nav.Select(
			  "//d:Tags", manager);
			logger.Log($"nodes is{(nodes.Count == 0 ? "" : " not")} empty");
			if (nodes.Count == 0)
			{
				XPathNodeIterator nodes2 = nav.Select("/");
				nodes2.MoveNext();
				logger.Log(nodes2.Current.InnerXml);
			}
			while (nodes.MoveNext())
			{
				tagsHandler.RecordTags(nodes.Current.InnerXml);
			}
			var next = nav.Select("//x:link[@rel='next']", manager);
			bool move1 = next.MoveNext();
			//bool move2 = next.Current.MoveToAttribute("href", "x");
			bool move3 = next.Current.MoveToFirstAttribute();
			bool move4 = next.Current.MoveToNextAttribute();
			logger.Log($"move1={move1} move3={move3} move4={move4}");
			string rtn = next.Current.Value;
			logger.Log(rtn);
			return rtn;	
		}
	}
}	
