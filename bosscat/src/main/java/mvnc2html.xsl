<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output indent="yes" method="xml" xml:space="default" encoding="utf-8" omit-xml-declaration="yes"/>

    <!--
        This stylesheet filters the output from the mc_auto program.  It attempts to unflatten
        the document to produce a hierarchy of sections and paragraphs.  It works but it is too slow.
        It expects input in the following format:
            <doc>
              <p class="mc_body" level="10">Narative</p>
              <p class="mc_tech" level="10" group="True">Style for quoted xml elements</p>
              <p class="mc_body" level="10">Reference to stuff stuff</p>
            </doc>
    -->
    <xsl:template match="doc">
        <xsl:element name="html">
            <xsl:element name="head">
                <xsl:element name="meta">
                    <xsl:attribute name="charset">utf-8</xsl:attribute>
                </xsl:element>
            </xsl:element>
            <xsl:element name="body">
                <xsl:apply-templates select="p[@level = 1]"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>


    <xsl:template match="p[@level = 10 and not(@group)]">
        <xsl:call-template name="handle_paragraph"/>
    </xsl:template>

    <!--
        Handle the first element in a set of elements where group=true
    -->
    <xsl:template match="p[@level = 10 and @group and preceding-sibling::p[1][not(@group)]]">
        <xsl:element name="SUMMARY">
            <xsl:call-template name="handle_paragraph"/>
            <xsl:apply-templates select="following-sibling::p[@group
              and generate-id(preceding-sibling::p[preceding-sibling::p[1][not(@group)]][1]) = generate-id(current())]" mode="grouped"/>
        </xsl:element>
    </xsl:template>

    <!--
        Handle the second and subsequent elements in a set of elements where group=true
    -->
    <xsl:template match="p[@level = 10 and @group and preceding-sibling::p[1][@group]]" mode="grouped">
        <xsl:element name="XYXY">
            <xsl:value-of select="."/>
        </xsl:element>
    </xsl:template>

    <!--
        suppress default processing of second and subsequent "grouped" elements as these
        are explicitly processed as following siblings of the first grouped element
    -->
    <xsl:template match="p[@level = 10 and @group and preceding-sibling::p[1][@group]]"/>

    <!--
        Recursively process the higher level elements.  In each case
        we first process any following body (level=10) elements.
    -->
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
            <xsl:call-template name="handle_paragraph"/>
            <xsl:apply-templates select="following-sibling::p[@level = 10
              and generate-id(preceding-sibling::p[@level &lt; 10][1]) = generate-id(current())]"/>"
            <xsl:apply-templates select="following-sibling::p[@level = $mylevel+1
              and generate-id(preceding-sibling::p[@level = $mylevel][1]) = generate-id(current())]"/>
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