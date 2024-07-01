﻿<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server">
    <!-- page layout: TransoilFrontPage -->
	<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/ru-ru/Core Styles/page-layouts-21.css %>" runat="server"/>
	<PublishingWebControls:EditModePanel runat="server">
		<!-- Styles for edit mode only-->
		<SharePointWebControls:CssRegistration name="<% $SPUrl:~sitecollection/Style Library/ru-ru/Core Styles/edit-mode-21.css %>"
			After="<% $SPUrl:~sitecollection/Style Library/ru-ru/Core Styles/page-layouts-21.css %>" runat="server"/>

        <script type="text/javascript">
            // устанавливаем текущую дату в поле Дата Статьи
            $(function() 
            {
                var articleStartDate = $('#ctl00_PlaceHolderMain_ctl01_ctl00_ctl00_DateTimeField_DateTimeFieldDate').val();
                if (!articleStartDate.length)
                {
                    var currentDate = new Date();
                    var day = currentDate.getDate();
                    var month = currentDate.getMonth() + 1;
                    var year = currentDate.getFullYear();
                    $('#ctl00_PlaceHolderMain_ctl01_ctl00_ctl00_DateTimeField_DateTimeFieldDate').val(day + "." + month + "." + year);
                }
            });
        </script>

	</PublishingWebControls:EditModePanel>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">	
    <SharePointWebControls:EncodedLiteral runat="server" text="Трансойл - " />
	<SharePointWebControls:ProjectProperty Property="Title" runat="server" />		
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<SharePointWebControls:FieldValue FieldName="Title" runat="server" Visible="false" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
    <!-- стили загружаемые в конце заголовка страницы -->
  <link rel="stylesheet" type="text/css" href="/Style Library/CompanyName/CSS/frontpage.css" />
  
    <!--[if lt IE 9]>
        <style type="text/css">     		
            .hotnews_text {
                    background:transparent;
                    filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#30000000,endColorstr=#30000000);
                    zoom: 1;
            }		
        </style>
	<![endif]-->
    <script type="text/javascript" src="/Style Library/CompanyName/Scripts/frontpage.js" ></script>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleBreadcrumb" runat="server"> 
    <SharePointWebControls:ListSiteMapPath runat="server" SiteMapProviders="CurrentNavigation" RenderCurrentNodeAsLink="false" PathSeparator="" CssClass="s4-breadcrumb" NodeStyle-CssClass="s4-breadcrumbNode" CurrentNodeStyle-CssClass="s4-breadcrumbCurrentNode" RootNodeStyle-CssClass="s4-breadcrumbRootNode" NodeImageOffsetX=0 NodeImageOffsetY=353 NodeImageWidth=16 NodeImageHeight=16 NodeImageUrl="/_layouts/images/fgimg.png" HideInteriorRootNodes="true" SkipLinkText="" /> 
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageBreadcrumb" runat="server" Visible="False"/>     

<asp:Content ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePointWebControls:ProjectProperty Property="Description" runat="server" Visible="false" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
	<div height=100% class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=10 height=1 alt=""></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
        
		 <table class="full_width_table main_page_table">
		  <tr>                
				<td class="left_column">
					<WebPartPages:WebPartZone runat="server" 
                        FrameType="TitleBarOnly" ID="CenterLeft" Title="<%$Resources:cms,WebPartZoneTitle_BottomRight%>" />
				</td>
                <td class="spacer_column">&nbsp;</td>
				<td class="middle_column">
					<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Center" Title="<%$Resources:cms,WebPartZoneTitle_Footer%>" />
				</td>
                <td class="spacer_column">&nbsp;</td>
                <td class="right_column">
					<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="CenterRight" Title="<%$Resources:cms,WebPartZoneTitle_CenterRight%>" />
				</td>                
		  </tr>
		  <tr>
				<td colspan="5">                    
					<WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="Bottom" Title="<%$Resources:cms,WebPartZoneTitle_Bottom%>" />                    
				</td>		   
		  </tr>
		 </table>
        
    <script type="text/javascript" src="/Style Library/CompanyName/Scripts/frontpage_startup.js"> </script>            

</asp:Content>