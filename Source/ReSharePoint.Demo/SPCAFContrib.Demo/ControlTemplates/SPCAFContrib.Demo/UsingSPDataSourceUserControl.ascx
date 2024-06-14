<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsingSPDataSourceUserControl.ascx.cs" Inherits="SPCAFContrib.Demo.ControlTemplates.SPCAFContrib.Demo.UsingSPDataSourceUserControl" %>

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

<SharePoint:SPDataSource runat="server" 
    ID="dsPersonTitles" DataSourceMode="List" 
    SelectCommand="<Query><OrderBy><FieldRef Name='SortOrder' Ascending='true' /></OrderBy></Query>">
    <SelectParameters>
      <asp:Parameter Name="WebUrl" DefaultValue="/configuration/" />
      <asp:Parameter Name="ListName" DefaultValue="PersonTitles" />
  </SelectParameters>
</SharePoint:SPDataSource> 

<asp:DropDownList runat="server" ID="ddlPersonTitles" CssClass="title" DataSourceID="dsPersonTitles" DataTextField="Title" DataValueField="ID">
</asp:DropDownList>