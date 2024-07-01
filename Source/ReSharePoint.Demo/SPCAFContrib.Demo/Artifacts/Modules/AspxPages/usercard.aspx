<%@ Register TagPrefix="WpNs0" Namespace="MOSS.WebParts" Assembly="MOSS.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1ac762bff3a69f4a"%>
<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document"  %>
<%@ Register tagprefix="WebPartPages" namespace="Microsoft.SharePoint.WebPartPages" assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register tagprefix="SharePoint" namespace="Microsoft.SharePoint.WebControls" assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register tagprefix="SPSolution" TagName="ManageProfileLinkVisibilityUserControl" Src="~/_controltemplates/TransOil.Intra/ManageProfileLinkVisibilityUserControl.ascx"%>

<asp:Content id="Content1" runat="server" contentplaceholderid="PlaceHolderMain">
<asp:ScriptManagerProxy runat="server" id="ScriptManagerProxy">
</asp:ScriptManagerProxy>
<WebPartPages:SPProxyWebPartManager runat="server" id="spproxywebpartmanager">
</WebPartPages:SPProxyWebPartManager>
<table width="100%"style="margin-top:-17px;margin-left:-5px;">
	<tr>
		<td style="width:70%" valign="top">
		<WebPartPages:DataFormWebPart runat="server" Description="" ImportErrorMessage="Невозможно импортировать эту веб-часть." PartOrder="2" HelpLink="" AllowRemove="True" IsVisible="True" AllowHide="True" UseSQLDataSourcePaging="True" ExportControlledProperties="True" DataSourceID="" Title="UserCard0" ViewFlag="8" NoDefaultStyle="TRUE" AllowConnect="True" FrameState="Normal" PageSize="1" PartImageLarge="" AsyncRefresh="True" ExportMode="All" Dir="Default" DetailLink="" ShowWithSampleData="False" ListId="00000000-0000-0000-0000-000000000000" ListName="" FrameType="None" PartImageSmall="" IsIncluded="True" SuppressWebPartChrome="False" AllowEdit="True" ManualRefresh="False" ChromeType="None" AutoRefresh="False" AutoRefreshInterval="60" AllowMinimize="True" ViewContentTypeId="" InitialAsyncDataFetch="False" MissingAssembly="Невозможно импортировать эту веб-часть." HelpMode="Modeless" ID="g_aaf91e45_dd7f_4745_ae18_450d171af719" ConnectionID="00000000-0000-0000-0000-000000000000" AllowZoneChange="True" IsIncludedFilter="" __MarkupType="vsattributemarkup" __WebPartId="{AAF91E45-DD7F-4745-AE18-450D171AF719}" __AllowXSLTEditing="true" WebPart="true" Height="" Width=""><ParameterBindings>
			<ParameterBinding Name="dvt_apos" Location="Postback;Connection"/>
			<ParameterBinding Name="ManualRefresh" Location="WPProperty[ManualRefresh]"/>
			<ParameterBinding Name="UserID" Location="CAMLVariable" DefaultValue="CurrentUserName"/>
			<ParameterBinding Name="Today" Location="CAMLVariable" DefaultValue="CurrentDate"/>
			<ParameterBinding Name="dvt_firstrow" Location="Postback;Connection"/>
			<ParameterBinding Name="dvt_nextpagedata" Location="Postback;Connection"/>
			<ParameterBinding Name="Search" Location="QueryString(Search)"/>
		</ParameterBindings>
<DataFields>
@PartitionId,PartitionId;@TermId,TermId;@LCID,LCID;@Label,Label;@IsDefault,IsDefault;</DataFields>
<Xsl>
<xsl:stylesheet xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal">
	<xsl:output method="html" indent="no"/>
	<xsl:decimal-format NaN=""/>
	<xsl:param name="dvt_apos">&apos;</xsl:param>
	<xsl:param name="ManualRefresh"></xsl:param>
	<xsl:param name="dvt_firstrow">1</xsl:param>
	<xsl:param name="dvt_nextpagedata" />
	<xsl:param name="Search">439</xsl:param>
	<xsl:variable name="dvt_1_automode">0</xsl:variable>
	<xsl:template match="/" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:SharePoint="Microsoft.SharePoint.WebControls">
		<xsl:choose>
			<xsl:when test="($ManualRefresh = 'True')">
				<table width="100%" border="0" cellpadding="0" cellspacing="0" >
					<tr>
						<td valign="top">
							<xsl:call-template name="dvt_1"/>
						</td>
						<td width="1%" class="ms-vb" valign="top">
							<img src="/_layouts/images/staticrefresh.gif" id="ManualRefresh" border="0" onclick="javascript: {ddwrt:GenFireServerEvent('__cancel')}" alt="Click here to refresh the dataview."/>
						</td>
					</tr>
				</table>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="dvt_1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>	
	<xsl:template name="dvt_1">
		<xsl:variable name="dvt_StyleName">RepForm3</xsl:variable>
		<xsl:variable name="Rows" select="/dsQueryResponse/UserCard0/NewDataSet/Row"/>
		
		<xsl:variable name="dvt_RowCount" select="count($Rows)"/>
		<xsl:variable name="RowLimit" select="1" />
		<xsl:variable name="FirstRow" select="$dvt_firstrow" />
		<xsl:variable name="LastRow">
			<xsl:choose>
				<xsl:when test="($FirstRow + $RowLimit - 1) &gt; $dvt_RowCount"><xsl:value-of select="$dvt_RowCount" /></xsl:when>
				<xsl:otherwise><xsl:value-of select="$FirstRow + $RowLimit - 1" /></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="IsEmpty" select="$dvt_RowCount = 0" />
		<xsl:variable name="dvt_IsEmpty" select="$dvt_RowCount = 0"/>
		
		<xsl:choose>
			<xsl:when test="$dvt_IsEmpty">
				<xsl:call-template name="dvt_1.empty"/>
			</xsl:when>
			<xsl:otherwise>
				<table border="0" width="100%">
					<xsl:call-template name="dvt_1.body">
						<xsl:with-param name="Rows" select="$Rows[position() &gt;= $FirstRow and position() &lt;= $LastRow]"/>
						<xsl:with-param name="FirstRow" select="1" />
						<xsl:with-param name="LastRow" select="$dvt_RowCount" />
					</xsl:call-template>
				</table>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:call-template name="dvt_1.commandfooter">
			<xsl:with-param name="FirstRow" select="$FirstRow" />
			<xsl:with-param name="LastRow" select="$LastRow" />
			<xsl:with-param name="RowLimit" select="$RowLimit" />
			<xsl:with-param name="dvt_RowCount" select="$dvt_RowCount" />
			<xsl:with-param name="RealLastRow" select="number(ddwrt:NameChanged('',-100))" />
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="dvt_1.body">
		<xsl:param name="Rows"/>
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:for-each select="$Rows">
			<xsl:variable name="dvt_KeepItemsTogether" select="false()" />
			<xsl:variable name="dvt_HideGroupDetail" select="false()" />
			<xsl:if test="(position() &gt;= $FirstRow and position() &lt;= $LastRow) or $dvt_KeepItemsTogether">
				<xsl:if test="not($dvt_HideGroupDetail)" ddwrt:cf_ignore="1">
					<xsl:call-template name="dvt_1.rowview" />
				</xsl:if>
			</xsl:if>
		</xsl:for-each>
		
	</xsl:template>
	<xsl:template name="dvt_1.rowview">
		<xsl:variable name="FullName" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '7']/@PropertyVal" />
		</xsl:variable>
		<xsl:variable name="FirstName" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = @RecordID and @PropertyID = 4]/@PropertyVal" />
		</xsl:variable>
		<xsl:variable name="LastName" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = @RecordID and @PropertyID = 5]/@PropertyVal" />
		</xsl:variable>
		<xsl:variable name="JobTitle" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '5059']/@SecondaryVal" />
		</xsl:variable>
		<xsl:variable name="Organization" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '10005']/@PropertyVal" />
		</xsl:variable>
		<xsl:variable name="Department" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '10002']/@SecondaryVal" />
		</xsl:variable>
		<xsl:variable name="DepartmentPath" >
			<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '10002']/@Path" />
		</xsl:variable>
		<xsl:variable name="CalculatedFullName" >
			<xsl:choose>
				<xsl:when test="$FullName = ''">
					<xsl:value-of select="concat($FirstName,concat(' ',$LastName))" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$FullName" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>	
		<tr style="font-size:12px;">
			<td style="color:#333;font-family:Verdana;font-size:12px;line-height:144%;border:1px silver solid">
			<table width="100%">
					<tr>
						<td style="width:144px; " valign="top">
						<div style="margin:25px;overflow:hidden">
						<xsl:if test="/dsQueryResponse/UserCard0/NewDataSet/Row [ @RecordID = $Search and @PropertyID = '23' ] /@PropertyVal != ''">
							<img src="{/dsQueryResponse/UserCard0/NewDataSet/Row[ @RecordID = $Search  and @PropertyID = '23']/@PropertyVal}" width="144px" style="border:1px silver solid;" />
						</xsl:if>
						<xsl:if test="/dsQueryResponse/UserCard0/NewDataSet/Row [ @RecordID = $Search and @PropertyID = '23' ] /@PropertyVal = ''">
							<img src="/phonebook/images/O14_person_placeHolder_240.png" width="144px" style="border:1px silver solid;" />
						</xsl:if>

						</div>
						</td>
						<td valign="top" style="padding-top:25px">
						<div style="font-size:12px">
						<table style="width:100%">
						<tr>
						<td>
							<h1 style="color:#333;font-size:18px; margin:0"><xsl:value-of select="$CalculatedFullName"/></h1>
						</td>
						<td style="text-align:right">
							<div style="margin-right:10px; color:#999;">
								<xsl:value-of select="$Organization"/>
							</div>
						</td>
						</tr>
						</table>
						<h2 style="color:#333;font-size:16px;margin-top:25px; margin-bottom:15px;font-weight:bold;"><xsl:value-of select="$JobTitle"/></h2>
						<div style="font-size:12px; margin-bottom:15px;"><xsl:value-of select="$Department"/></div>
						
						<xsl:call-template name="Dep">
								<xsl:with-param name="Dep" select="$DepartmentPath" />
						</xsl:call-template>

						</div>
						
						</td>
					</tr>
				</table>
				<div style="margin-left:16px; font-size:12px;">
							<h2 style="color:#333; margin-left:10px; margin-top:10px; margin-bottom:5px;font-size:16px; font-weight:bold;">Основная информация</h2>							
							<table cellspacing="10" >
								<tr style="font-size:12px;">
									<td style="width: 164px;">E-mail:</td>
									<td>
										<a title="" href="mailto:{/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '9']/@PropertyVal}" target="">
											<xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '9']/@PropertyVal" />
										</a>
									</td>
								</tr>
								<tr>
									<td style="width: 164px">Телефон:</td>
									<td><xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '8']/@PropertyVal" /></td>
								</tr>
								<tr>
									<td style="width: 164px">Кабинет:</td>
									<td><xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '11']/@PropertyVal" /></td>
								</tr>		
							</table>						
						</div>
				<div style="margin-left:16px; font-size:12px;">
					<h2 style="color:#333;font-size:16px;margin-left:10px;margin-top:15px;margin-bottom:5px; font-weight:bold;">Дополнительная информация</h2>
					<table cellspacing="10" >
						<tr style="font-size:12px;">
							<td style="width: 164px;">Городской номер:</td>
							<td><xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '18']/@PropertyVal" /></td>
						</tr>
						<tr>
							<td style="width: 164px">Мобильный номер:</td>
							<td><xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '19']/@PropertyVal" /></td>
						</tr>
						<tr>
							<td style="width: 164px">Адрес:</td>
							<td><xsl:value-of select="/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '10006']/@PropertyVal" /></td>
						</tr>
						<tr>
							<td style="width: 164px">День рождения:</td>
							<td><xsl:value-of select="ddwrt:FormatDateTime(string(/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '10008']/@PropertyVal) ,1049 , 'dd MMMM')" /></td>
						</tr>
						<tr>
							<td style="width: 164px">В компании с:</td>
							<td><xsl:value-of select="ddwrt:FormatDate(string(/dsQueryResponse/UserCard0/NewDataSet/Row[@RecordID = $Search and @PropertyID = '5014']/@PropertyVal) ,1049 , 3)" /></td>
						</tr>
					</table>
				</div>
				<div style="margin-left:16px; font-size:12px;">
					<div class="edit_profile_link">		
						<a style="color:#999;" href="http://spbsrvmoss:7777/_layouts/ProfAdminEdit.aspx?guid={/dsQueryResponse/UserCard0/NewDataSet/Row [ @RecordID = $Search and @PropertyID = '1' ] /@PropertyVal}&amp;ConsoleView=Active&amp;ProfileType=User&amp;ApplicationID=279fe750%2Df96f%2D4a5b%2Dac38%2D886139d118d1">Редактировать профиль</a>
					</div>
					<div class="last_visit_date">
						<iframe	src="/_layouts/Transoil.Intra/lastvisit.aspx?rid={@RecordID}" width="300px" height="15px" frameborder="no" scrolling="no" />							
					</div>
				</div>
			</td>
		</tr>
		
	</xsl:template>	
	<xsl:template name="Dep">
		<xsl:param name="Dep"/>
		<div style="font-size:12px;">
			<xsl:if test="substring-before($Dep,'\') = ''">
				<div ><xsl:value-of select="/dsQueryResponse/meta/NewDataSet/Row[@TermId = $Dep]/@Label" /></div>
			</xsl:if>
			<xsl:if test="substring-before($Dep,'\') != ''">
			<div style="margin-bottom:15px;padding-right:10px"><xsl:value-of select="/dsQueryResponse/meta/NewDataSet/Row[@TermId = substring-before($Dep,'\')]/@Label" /></div>
			</xsl:if>
			<xsl:if test="substring-after($Dep,'\') != ''">
				<xsl:call-template name="Dep">
					<xsl:with-param name="Dep" select="substring-after($Dep,'\')" />
				</xsl:call-template>

				<div style="display:none"><xsl:value-of select="substring-after($Dep,' ')" /></div>
			</xsl:if>
		</div>
	</xsl:template>
	<xsl:template name="dvt_1.empty">
		<xsl:variable name="dvt_ViewEmptyText">Нет элементов для отображения в этом представлении.</xsl:variable>
		<table border="0" width="100%">
			<tr>
				<td class="ms-vb">
					<xsl:value-of select="$dvt_ViewEmptyText"/>
				</td>
			</tr>
		</table>
	</xsl:template>
	<xsl:template name="dvt_1.commandfooter">
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:param name="RowLimit" />
		<xsl:param name="dvt_RowCount" />
		<xsl:param name="RealLastRow" />
		
	</xsl:template>
	<xsl:template name="dvt_1.navigation">
		<xsl:param name="FirstRow" />
		<xsl:param name="LastRow" />
		<xsl:param name="RowLimit" />
		<xsl:param name="dvt_RowCount" />
		<xsl:param name="RealLastRow" />
		<xsl:variable name="PrevRow">
			<xsl:choose>
				<xsl:when test="$FirstRow - $RowLimit &lt; 1">1</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$FirstRow - $RowLimit" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="LastRowValue">
			<xsl:choose>
				<xsl:when test="$LastRow &gt; $RealLastRow">
					<xsl:value-of select="$LastRow"></xsl:value-of>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$RealLastRow"></xsl:value-of>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="NextRow">
			<xsl:value-of select="$LastRowValue + 1"></xsl:value-of>
		</xsl:variable>
		<td nowrap="nowrap" class="ms-paging" align="right">
			
			
			
			
			
		</td>
	</xsl:template>
</xsl:stylesheet>	</Xsl>
<DataSources>
<SharePoint:AggregateDataSource runat="server" IsSynchronous="" SeparateRoot="true" RootName="" RowsName="" ID="UserCard1"><Sources>
<SharePoint:SPSqlDataSource runat="server" AllowIntegratedSecurity="False" ConnectionString="Data Source=DATRUS-PC\SHAREPOINT;User ID=digdes;Password=8lKyTp3dDK;Initial Catalog=Profile DB;" ProviderName="System.Data.SqlClient" SelectCommand="dbo.[dd_UserCard]" SelectCommandType="StoredProcedure"><SelectParameters>
<asp:QueryStringParameter QueryStringField="Search" Name="uID"></asp:QueryStringParameter>
</SelectParameters>
</SharePoint:SPSqlDataSource>
<SharePoint:SPSqlDataSource runat="server" AllowIntegratedSecurity="False" ConnectionString="Data Source=DATRUS-PC\SHAREPOINT;User ID=digdes;Password=8lKyTp3dDK;Initial Catalog=Managed Metadata Service_04392a2b7d6343edb0062c901a689183;" ProviderName="System.Data.SqlClient" SelectCommand="SELECT [ECMTermLabel] . * FROM [ECMTermLabel] ORDER BY LABEL DESC"></SharePoint:SPSqlDataSource>
</Sources>
<Aggregate>

<concat name="data source"><datasource name="UserCard0" id="0" Type="Sql" /><datasource name="meta" id="1" Type="Sql" /></concat></Aggregate>
</SharePoint:AggregateDataSource>
</DataSources>
</WebPartPages:DataFormWebPart>

</ZoneTemplate></WebPartPages:WebPartZone>

</td>
		<td style="width:auto;" valign="top">
		<WebPartPages:WebPartZone id="g_C9EA530A61D94B568F6943EB3F5220B0" runat="server" title="Зона 2" __designer:Preview="&lt;Regions&gt;&lt;Region Name=&quot;0&quot; Editable=&quot;True&quot; Content=&quot;&quot; NamingContainer=&quot;True&quot; /&gt;&lt;/Regions&gt;&lt;table cellspacing=&quot;0&quot; cellpadding=&quot;0&quot; border=&quot;0&quot; id=&quot;g_C9EA530A61D94B568F6943EB3F5220B0&quot;&gt;
	&lt;tr&gt;
		&lt;td style=&quot;white-space:nowrap;&quot;&gt;&lt;table cellspacing=&quot;0&quot; cellpadding=&quot;2&quot; border=&quot;0&quot; style=&quot;width:100%;&quot;&gt;
			&lt;tr&gt;
				&lt;td style=&quot;white-space:nowrap;&quot;&gt;Зона 2&lt;/td&gt;
			&lt;/tr&gt;
		&lt;/table&gt;&lt;/td&gt;
	&lt;/tr&gt;&lt;tr&gt;
		&lt;td style=&quot;height:100%;&quot;&gt;&lt;table cellspacing=&quot;0&quot; cellpadding=&quot;2&quot; border=&quot;0&quot; style=&quot;border-color:Gray;border-width:1px;border-style:Solid;width:100%;height:100%;&quot;&gt;
			&lt;tr valign=&quot;top&quot;&gt;
				&lt;td _designerRegion=&quot;0&quot;&gt;&lt;table cellspacing=&quot;0&quot; cellpadding=&quot;2&quot; border=&quot;0&quot; style=&quot;width:100%;&quot;&gt;
					&lt;tr&gt;
						&lt;td style=&quot;height:100%;&quot;&gt;&lt;/td&gt;
					&lt;/tr&gt;
				&lt;/table&gt;&lt;/td&gt;
			&lt;/tr&gt;
		&lt;/table&gt;&lt;/td&gt;
	&lt;/tr&gt;
&lt;/table&gt;" __designer:Values="&lt;P N='ID' ID='1' T='g_C9EA530A61D94B568F6943EB3F5220B0' /&gt;&lt;P N='HeaderText' ID='2' T='Зона 2' /&gt;&lt;P N='DisplayTitle' R='2' /&gt;&lt;P N='Title' R='2' /&gt;&lt;P N='Page' ID='3' /&gt;&lt;P N='TemplateControl' R='3' /&gt;&lt;P N='AppRelativeTemplateSourceDirectory' T='~/' /&gt;" __designer:Templates="&lt;Group Name=&quot;ZoneTemplate&quot;&gt;&lt;Template Name=&quot;ZoneTemplate&quot; Content=&quot;&quot; /&gt;&lt;/Group&gt;"><ZoneTemplate><WpNs0:OrganizationTreeWebPart runat="server" TermSetName="" ImportErrorMessage="Не удается импортировать эту веб-часть." ExportMode="All" ChromeType="None" OrgstructureTreeDepth="4" Description="Представление организационной структуры в виде дерева" ID="g_23839840_9b46_429a_bc35_2e21c4ef25eb" GroupName="Трансойл" OrgstructureTreeLCID="0" PhonebookPageUrl="/phonebook/SitePages/UsersSearch.aspx" Title="Дерево подразделений" __designer:Values="&lt;P N='GroupName' T='Трансойл' /&gt;&lt;P N='TermSetName' R='-1' /&gt;&lt;P N='PhonebookPageUrl' T='/phonebook/SitePages/UsersSearch.aspx' /&gt;&lt;P N='OrgstructureTreeDepth' T='4' /&gt;&lt;P N='OrgstructureTreeLCID' T='0' /&gt;&lt;P N='ChromeType' E='2' /&gt;&lt;P N='Description' T='Представление организационной структуры в виде дерева' /&gt;&lt;P N='DisplayTitle' ID='1' T='Дерево подразделений' /&gt;&lt;P N='ExportMode' E='1' /&gt;&lt;P N='ImportErrorMessage' T='Не удается импортировать эту веб-часть.' /&gt;&lt;P N='IsShared' T='True' /&gt;&lt;P N='IsStandalone' T='False' /&gt;&lt;P N='IsStatic' T='False' /&gt;&lt;P N='Title' R='1' /&gt;&lt;P N='WebBrowsableObject' R='0' /&gt;&lt;P N='ZoneIndex' T='1' /&gt;&lt;P N='ID' T='g_23839840_9b46_429a_bc35_2e21c4ef25eb' /&gt;&lt;P N='Page' ID='2' /&gt;&lt;P N='TemplateControl' R='2' /&gt;&lt;P N='AppRelativeTemplateSourceDirectory' T='~/' /&gt;" __designer:Preview="&lt;table class=&quot;s4-wpTopTable&quot; border=&quot;0&quot; cellpadding=&quot;0&quot; cellspacing=&quot;0&quot; width=&quot;100%&quot;&gt;
	&lt;tr&gt;
		&lt;td valign=&quot;top&quot;&gt;&lt;div WebPartID=&quot;&quot; HasPers=&quot;false&quot; id=&quot;WebPartg_C9EA530A61D94B568F6943EB3F5220B0_g_23839840_9b46_429a_bc35_2e21c4ef25eb&quot; width=&quot;100%&quot; class=&quot;ms-WPBody&quot; allowDelete=&quot;false&quot; style=&quot;&quot; &gt;&lt;div id=&quot;WebPartContent&quot;&gt;
			&lt;div id=&quot;g_C9EA530A61D94B568F6943EB3F5220B0_g_23839840_9b46_429a_bc35_2e21c4ef25eb&quot;&gt;
	

&lt;link href=&quot;/_layouts/MOSS.Common/dynatree/skin-vista/ui.dynatree.css&quot; rel=&quot;stylesheet&quot; type=&quot;text/css&quot;&gt;

&lt;style type=&quot;text/css&quot;&gt;
ul.dynatree-container {
	height: 100%;
	width: 100%;
	background-color: transparent;
	border:0;	
	overflow-x: hidden;
	padding:0;	
}

.dynatree-title
{
    white-space:normal;
	word-wrap: break-word;
}

.ms-WPBody dir li:before, .ms-WPBody ul li:before
{
    content:&quot;&quot;;
}
&lt;/style&gt;

&lt;div id=&quot;tree&quot;&gt;
    &lt;ul id=&quot;treeData&quot; style=&quot;display: none;&quot;&gt;
    &lt;li&gt;&lt;a id=&quot;g_C9EA530A61D94B568F6943EB3F5220B0_g_23839840_9b46_429a_bc35_2e21c4ef25eb_ctl00_lnkRoot&quot; target=&quot;_self&quot;&gt;Все сотрудники&lt;/a&gt;   
        &lt;ul&gt;
            
        &lt;/ul&gt;
    &lt;/li&gt;
    &lt;/ul&gt;
&lt;/div&gt;

&lt;script type=&quot;text/javascript&quot;&gt;
    $(function(){
        var tree = $(&quot;#tree&quot;).dynatree({    
        minExpandLevel: 4,    
        onActivate: function (node) {
            if (node.data.href) {
                window.open(node.data.href, node.data.target);
            }
        }
      });      
    });
  &lt;/script&gt;

&lt;/div&gt;
		&lt;/div&gt;&lt;/div&gt;&lt;/td&gt;
	&lt;/tr&gt;
&lt;/table&gt;" __MarkupType="vsattributemarkup" __WebPartId="{23839840-9B46-429A-BC35-2E21C4EF25EB}" WebPart="true" __designer:IsClosed="false"></WpNs0:OrganizationTreeWebPart>

</ZoneTemplate></WebPartPages:WebPartZone></td>
	</tr>
</table>

</asp:Content>
<asp:Content id="Content2" runat="server" contentplaceholderid="PlaceHolderAdditionalPageHead">
	<style type="text/css">
	#s4-leftpanel
	{
		display:none;
	}
	.s4-ca
	{
		margin-left:0px;
	}
	#tree
	{
	    margin-top: 30px;
	}
	.dynatree-title
	{ 
		margin-right:30px !important;
	}
	.edit_profile_link
	{
		float:left;
		padding:10px; 
		margin-bottom:10px;
		display:none;
	}
	.last_visit_date
	{
		float:right;
		margin-right:10px;	
		margin-top: 10px;
	}

	</style>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $(".left_menu").accordion({
                accordion: true,
                speed: 500,
                closedSign: '[+]',
                openedSign: '[-]'
            });
        });
    </script>

	<SPSolution:ManageProfileLinkVisibilityUserControl runat="server" ID="ctlManageProfileLinkVisibilityU" __designer:Preview="[ Literal &quot;ltr&quot; ]
" __designer:Values="&lt;P N='ID' ID='1' T='ctlManageProfileLinkVisibilityU' /&gt;&lt;P N='TemplateControl' R='0' /&gt;"></SPSolution:ManageProfileLinkVisibilityUserControl>							
	
	</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="Трансойл - " __designer:Preview="Трансойл - " __designer:Values="&lt;P N='Text' T='Трансойл - ' /&gt;&lt;P N='ID' ID='1' T='ctl00' /&gt;&lt;P N='Page' ID='2' /&gt;&lt;P N='TemplateControl' R='2' /&gt;&lt;P N='AppRelativeTemplateSourceDirectory' T='~/' /&gt;"/>	Карточка 
	
	
	
	
	сотрудника
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderPageBreadcrumb" runat="server">
	<span id="ctl00_PlaceHolderPageBreadcrumb_ContentMap" hideinteriorrootnodes="false">
    	<span><a class="ms-sitemapdirectional" href="/">Главная</a></span>
    	<span><img src="/_layouts/images/TransOil.Intra/bread_crops_narrow.png" class="breadcrumb_narrow"></span>
    	<span class="ms-sitemapdirectional"><a class="ms-sitemapdirectional" href="/phonebook">
	Телефонный справочник</a></span>
	    	<span><img src="/_layouts/images/TransOil.Intra/bread_crops_narrow.png" class="breadcrumb_narrow"></span>
		<span class="ms-sitemapdirectional">
		Карточка сотрудника</span>

    </span> 
</asp:Content>
