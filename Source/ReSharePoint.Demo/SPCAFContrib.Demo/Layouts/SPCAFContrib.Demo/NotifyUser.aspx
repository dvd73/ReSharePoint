<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotifyUser.aspx.cs" Inherits="SPCAFContrib.Demo.Layouts.NotifyUser" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
	Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web"%>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html lang="<%$Resources:wss,language_value%>" dir="<%$Resources:wss,multipages_direction_dir_value%>" runat="server" xmlns:o="urn:schemas-microsoft-com:office:office">
<head runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint">
	<meta name="progid" content="SharePoint.WebPartPage.Document">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta http-equiv="Expires" content="0">
	<meta http-equiv="X-UA-Compatible" content="IE=8" />
	<title>User meeting notification</title>
	<SharePoint:CssLink ID="CssLink1" runat="server" />

	<style>
		
		#progressBackgroundFilter {
			position:fixed;
			top:0px;
			bottom:0px;
			left:0px;
			right:0px;
			overflow:hidden;
			padding:0;
			margin:0;
			background-color:#000;
			filter:alpha(opacity=50);
			opacity:0.5;
			z-index:1000;
		}
		#processMessage {
			position:fixed;
			top:20%;
			left:40%;
			padding:10px;
			width:14%;
			z-index:1001;
			background-color:#fff;
			border:solid 1px #000;
		}
		
	</style>

</head>

<body scroll="no" class="v4master">
<form runat="server" id="frmMain">
<div class="ms-bodyareaframe" style="height:100%;">

<asp:ScriptManager runat="server"/>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
   <ContentTemplate>

<table cellpadding="0" cellspacing="0" style="width: 100%;">
	<tr>
		<td>
			<asp:HiddenField runat="server" ID="hfMeetingTitle"/>
			<b><asp:Literal runat="server" Text="<%$Resources:berggren_site, ui_message%>" /></b> <br/>
			<asp:TextBox runat="server" ID="tbMessage" TextMode="MultiLine" Width="100%" style="overflow: hidden;" Rows="5" />
		</td>
	</tr>
	<tr>
		<td>
			<br/>
			<br/>
			<asp:Repeater runat="server" ID="rptRecepients" OnItemDataBound="rptRecepients_ItemDataBound">
			<HeaderTemplate>
				<table cellpadding="0" cellspacing="0" style="width:100%">
					<tr>
						<td> <b> <asp:Literal runat="server" Text="<%$Resources:berggren_site, ui_recipient%>" /> </b></td>
						<td> <b> <asp:Literal runat="server" Text="<%$Resources:berggren_site, ui_email%>" /> </b></td>
						<td> <b> <asp:Literal runat="server" Text="<%$Resources:berggren_site, ui_selected%>" /> </b></td>
						<td> <b> <asp:Literal runat="server" Text="<%$Resources:berggren_site, ui_note%>" /> </b></td>
						<td> </td>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<asp:Label runat="server" Text='<%# Eval("FullName") %>' ID="ltrName"/>
						<asp:HiddenField runat="server" ID="hfUserId" Value='<%# Eval("RecipientId") %>'/>
					</td>
					<td>
						<asp:Label runat="server" Text='<%# Eval("EMail") %>' ID="ltrEMail"/>
					</td>
					<td>
						<asp:Checkbox runat="server" Checked='<%#  Eval("Resolved") %>' ID="cbSelected"/>
					</td>
					<td>
						<asp:Label runat="server" Text='<%# Eval("ResolveInfo") %>' ID="lblResolveInfo"/>
						<asp:HiddenField runat="server" ID="hfIsOrganizer" Value='<%# Eval("IsOrganizer") %>'/>
					</td>
					<td><asp:Image runat="server" ID="imgSendStatus" Visible="false"/></td>
				</tr>   
			</ItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate> 
			</asp:Repeater> 
		</td> 
	</tr>
	<tr>
	<td align="right">
		<br/>
		<br/>
		<asp:Button runat="server" ID="btnSend" Text="<%$Resources:berggren_site, ui_button_send%>" CssClass="ms-ButtonHeightWidth" OnClick="btnSend_Click"/>
		&nbsp;&nbsp;
		<asp:Button runat="server" ID="btnClose" Text="<%$Resources:berggren_site, ui_button_close%>" CssClass="ms-ButtonHeightWidth"/>
	</td>
	</tr>
</table>
	</ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
	<ProgressTemplate>

		<div id="progressBackgroundFilter"></div>
		<div id="processMessage"> &nbsp;&nbsp;<asp:Literal runat="server" Text="<%$Resources:berggren_site, ui_sending%>" /><br /><br />
			 <asp:Image runat="server" ImageUrl="<% $SPUrl:~site/_layouts/TrainersHouse/images/ajax-loader.gif %>" />
		</div>
				
	</ProgressTemplate>
</asp:UpdateProgress>

</div>
</form>
</body>
</html>