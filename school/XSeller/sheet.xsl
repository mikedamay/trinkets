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
