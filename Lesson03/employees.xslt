<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="xml" encoding="utf-8" indent="yes"/>

  <xsl:template match="/">
    <COMPANY>
      <xsl:for-each select="employees/employee">
        <EMPLOYEE>
          <xsl:attribute name="TOWN">
            <xsl:value-of select="town"/>
          </xsl:attribute>
          <xsl:value-of select="name"/>
          <xsl:for-each select="degree">
            <TITLE>
              <xsl:value-of select="."/>
              <xsl:text> (</xsl:text>
              <xsl:value-of select="@level"/>
              <xsl:text>)</xsl:text>
            </TITLE>
          </xsl:for-each>
        </EMPLOYEE>
      </xsl:for-each>
    </COMPANY>
  </xsl:template>

</xsl:stylesheet>
