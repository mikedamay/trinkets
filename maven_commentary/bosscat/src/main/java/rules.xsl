<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/rules">
        <xsl:copy>
            <xsl:apply-templates select="if"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="if">
        <conditionSet>
            <xsl:copy-of select="."/>
            <xsl:apply-templates select="
                             following-sibling::*[not(self::if)
                             and preceding-sibling::if[1]
                                = current()]
                             "/>
        </conditionSet>
    </xsl:template>
<!--
    <xsl:template match="if">
        <conditionSet>
            <xsl:copy-of select="."/>
            <xsl:apply-templates select="
                             following-sibling::*[not(self::if)
                             and generate-id(preceding-sibling::if[1])
                                = generate-id(current())]
                             "/>
        </conditionSet>
    </xsl:template>
-->

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>