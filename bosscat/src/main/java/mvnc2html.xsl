<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output indent="yes" method="xml" xml:space="default" encoding="utf-8" omit-xml-declaration="yes"/>
    
    <xsl:template match="doc">
        <xsl:element name="html">
            <xsl:element name="head">
                <xsl:element name="meta">
                    <xsl:attribute name="charset">utf-8</xsl:attribute>
                </xsl:element>
            </xsl:element>
            <xsl:element name="body">
                <!--<xsl:apply-templates/>-->
                <xsl:apply-templates select="p[@level = 1]"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>


    <xsl:template match="p[@level = 10 and not(@group)]">
        <xsl:call-template name="handle_paragraph"/>
    </xsl:template>

    <xsl:template match="p[@level = 10 and @group and preceding-sibling::p[1][not(@group)]]">
        <xsl:element name="SUMMARY">
            <xsl:call-template name="handle_paragraph"/>
            <xsl:apply-templates select="following-sibling::p[@group
              and preceding-sibling::p[preceding-sibling::p[1][not(@group)]][1] = current()]" mode="grouped"/>
        </xsl:element>
    </xsl:template>

    <xsl:template match="p[@level = 10 and @group and preceding-sibling::p[1][@group]]" mode="grouped">
        <xsl:element name="XYXY">
            <xsl:value-of select="."/>
        </xsl:element>
    </xsl:template>

    <xsl:template match="p[@level = 10 and @group and preceding-sibling::p[1][@group]]"/>

    <xsl:template match="p[@level &lt; 10]">
        <xsl:variable name="mylevel">
            <xsl:value-of select="@level"/>
        </xsl:variable>
        <xsl:element name="div">
            <xsl:attribute name="class">
                <xsl:text>level</xsl:text><xsl:value-of select="@level"/>
            </xsl:attribute>
            <xsl:attribute name="myy">
                <xsl:value-of select="$mylevel"/>
            </xsl:attribute>
            <xsl:attribute name="num-sub-paras">
                <xsl:value-of select="count(following-sibling::p[@level &gt; $mylevel and preceding-sibling::p[@level = $mylevel][position() = 1] = current()])"/>
            </xsl:attribute>
            <xsl:call-template name="handle_paragraph"/>
            <xsl:apply-templates select="following-sibling::p[@level = 10
              and preceding-sibling::p[@level &lt; 10][1] = current()]"/>"
            <xsl:apply-templates select="following-sibling::p[@level = $mylevel+1
              and preceding-sibling::p[@level = $mylevel][1] = current()]"/>
        </xsl:element>
    </xsl:template>

    <xsl:template name="handle_paragraph">
        <xsl:element name="div">
            <xsl:attribute name="class">
                <xsl:value-of select="@class"/>
            </xsl:attribute>
            <xsl:apply-templates/>
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>