<?xml version="1.0" encoding="us-ascii" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:tt="http://www.w3.org/2005/Atom"
xmlns:d="http://schemas.microsoft.com/ado/2007/08/dataservices">
    <xsl:output encoding="us-ascii" indent="yes"
    />
    <!-- this is bad.  It will only show the first child -->
    <xsl:template match="abc">
        <xsl:value-of select="mychild"/>
    </xsl:template>
    <xsl:template match="tt:link[@rel='next']">
        <xsl:value-of select="@href"/>
    </xsl:template>
    <xsl:template match="text()"/>

</xsl:stylesheet>