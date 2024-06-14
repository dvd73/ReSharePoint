<%@ Control Language="C#"   AutoEventWireup="false" %>
<%@Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.WebControls"%>
<%@Register TagPrefix="ApplicationPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.ApplicationPages.WebControls"%>
<%@Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.Utilities"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="SPSolution" Assembly="$SharePoint.Project.AssemblyFullName$" namespace="SPCAFContrib.Demo.Common.Iterators"%>
<%@ Assembly Name="MOSS.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7e688b91a6577ddf" %>

<SharePoint:RenderingTemplate id="GTSSUserNotificationTemplateListForm" runat="server">
	<Template>
		<span id='part1'>
			<SharePoint:InformationBar ID="InformationBar1" runat="server"/>
			<div id="listFormToolBarTop">
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbltop" RightButtonSeparator="&amp;#160;" runat="server">
					<Template_RightButtons>
						<SharePoint:NextPageButton ID="NextPageButton1" runat="server"/>
						<SharePoint:SaveButton ID="SaveButton1" runat="server"/>
						<SharePoint:GoBackButton ID="GoBackButton1" runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</div>
			<SharePoint:FormToolBar ID="FormToolBar1" runat="server"/>
			<SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage1" runat="server"/>
			<table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0" width="100%">
			<SharePoint:ChangeContentType ID="ChangeContentType1" runat="server"/>
			<SharePoint:FolderFormFields ID="FolderFormFields1" runat="server"/>
			<SPSolution:UserNotificationTemplateListFieldIterator runat="server"/>
			<SharePoint:ApprovalStatus ID="ApprovalStatus1" runat="server"/>
			<SharePoint:FormComponent ID="FormComponent1" TemplateName="AttachmentRows" runat="server"/>
			</table>
			<table cellpadding="0" cellspacing="0" width="100%"><tr><td class="ms-formline"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr></table>
			<table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px"><tr><td width="100%">
			<SharePoint:ItemHiddenVersion ID="ItemHiddenVersion1" runat="server"/>
			<SharePoint:ParentInformationField ID="ParentInformationField1" runat="server"/>
			<SharePoint:InitContentType ID="InitContentType1" runat="server"/>
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbl" RightButtonSeparator="&amp;#160;" runat="server">
					<Template_Buttons>
						<SharePoint:CreatedModifiedInfo ID="CreatedModifiedInfo1" runat="server"/>
					</Template_Buttons>
					<Template_RightButtons>
						<SharePoint:SaveButton ID="SaveButton2" runat="server"/>
						<SharePoint:GoBackButton ID="GoBackButton2" runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</td></tr></table>
		</span>
		<SharePoint:AttachmentUpload ID="AttachmentUpload1" runat="server"/>
	</Template>
</SharePoint:RenderingTemplate>