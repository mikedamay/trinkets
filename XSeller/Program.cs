namespace XSeller
{
	using System;
	using PureDI;

	[Bean]
	public class Program
	{
		[BeanReference] private XSeller _xseller = null;
		public static void Main()
		{
			var pdi = new PDependencyInjector();
			(var prog, var @is) = pdi.CreateAndInjectDependencies<Program>();
			Console.WriteLine(@is.AllDiagnosticsToString());
			prog._xseller.XSell();
		}
	}
}
namespace XSeller
{
	using System;
	using PureDI;
	using System.IO;
	using System.Xml;
	using System.Xml.Xsl;
	
	[Bean]
	internal class XSeller
	{
		[BeanReference] private StylesheetProvider _stylesheetProvider = null;
		[BeanReference] private DocumentProvider _documentProvider = null;
		public void XSell()
		{
			var tf = new XslCompiledTransform();
			tf.Load(XmlReader.Create(_stylesheetProvider.ProvideSheet()));
			tf.Transform(XmlReader.Create(_documentProvider.ProvideDoc())
			  ,XmlWriter.Create(new StreamWriter("out.txt")
			  , new XmlWriterSettings() {CheckCharacters = false
			  , ConformanceLevel = ConformanceLevel.Fragment}));
		}
	}
}
namespace XSeller
{
	using System;
	using System.IO;
	using System.Text;
	using PureDI;

	[Bean]
	internal class StylesheetProvider
	{
		private string _sheet = @"
<?xml version='1.0' encoding='utf-8' ?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
  xmlns='http://TheDisappointedProgrammer.com/PureDI'>
	<output method='xml' indent='yes' encoding='utf-8'/>
	<xsl:template match='/'>
<out>
		<xsl:apply-templates/>
</out>
	</xsl:template>
</xsl:stylesheet>
		  ";
		public TextReader ProvideSheet()
		{
			return new StringReader(_sheet.Trim());
		}
	}
	[Bean]
	internal class DocumentProvider
	{
		private string _doc = @"
<?xml version='1.0' encoding='utf-8' ?>
<root>
	<part>this is a part</part>
</root>
		";
		public TextReader ProvideDoc()
		{
			return new StringReader(_doc.Trim());
		}
	}
}
