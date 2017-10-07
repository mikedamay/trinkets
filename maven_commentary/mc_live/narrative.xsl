<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" encoding="iso-8859-1"/>

    <xsl:template match="/doc">
        <html>
            <head>
                <link rel="stylesheet" type="text/css" href="mvn.css"/>
            </head>
            <body>
                <xsl:apply-templates select='*'/>
            </body>
        </html>
    </xsl:template>

    <xsl:template match='node() | @*'>
        <xsl:copy>
            <xsl:apply-templates select='node() |@*'/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match='p[child::p]'>
        <xsl:element name="div">
            <xsl:call-template name="replace-heading-attribute">
                <xsl:with-param name="class-name" select="child::p/@class"/>
            </xsl:call-template>
            <xsl:apply-templates select='node() |@*'/>
        </xsl:element>
    </xsl:template>

    <xsl:template name="replace-heading-attribute">
        <xsl:param name="class-name"/>
        <xsl:variable name="heading-text">Heading</xsl:variable>
        <xsl:variable name="level-text">Level</xsl:variable>
        <xsl:variable name="section-class-name">
            <xsl:call-template name="search-and-replace">
                <xsl:with-param name="input" select="$class-name" />
                <xsl:with-param name="search-string" select="$heading-text" />
                <xsl:with-param name="replace-string" select="$level-text" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:attribute name="class"><xsl:value-of select="$section-class-name"/></xsl:attribute>
    </xsl:template>

    <!-- from XSLT Cookbook -->
    <xsl:template name="search-and-replace">
        <xsl:param name="input"/>
        <xsl:param name="search-string"/>
        <xsl:param name="replace-string"/>
        <xsl:choose>
            <!-- See if the input contains the search string -->
            <xsl:when test="$search-string and
                           contains($input,$search-string)">
                <!-- If so, then concatenate the substring before the search
                string to the replacement string and to the result of
                recursively applying this template to the remaining substring.
                -->
                <xsl:value-of
                        select="substring-before($input,$search-string)"/>
                <xsl:value-of select="$replace-string"/>
                <xsl:call-template name="search-and-replace">
                    <xsl:with-param name="input"
                                    select="substring-after($input,$search-string)"/>
                    <xsl:with-param name="search-string"
                                    select="$search-string"/>
                    <xsl:with-param name="replace-string"
                                    select="$replace-string"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <!-- There are no more occurrences of the search string so
                just return the current input string -->
                <xsl:value-of select="$input"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>


</xsl:stylesheet>