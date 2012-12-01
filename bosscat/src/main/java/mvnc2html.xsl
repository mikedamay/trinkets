<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output indent="yes" method="xml" xml:space="default" encoding="utf-8" omit-xml-declaration="yes"/>
    
    <xsl:template match="/">
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="doc">
        <xsl:element name="html">
            <xsl:element name="head">
                <xsl:element name="meta">
                    <xsl:attribute name="charset">utf-8</xsl:attribute>
                </xsl:element>
            </xsl:element>
            <xsl:element name="body">
                <xsl:apply-templates/>
            </xsl:element>
        </xsl:element>
    </xsl:template>

    <xsl:template match="p">
        <xsl:element name="div">
            <xsl:apply-templates select="node() | @*"/>
        </xsl:element>
    </xsl:template>

    <xsl:template match="p[@level=8]">
        <xsl:element name="div">
            <xsl:element name="div">
                <xsl:apply-templates select="node() | @*"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>