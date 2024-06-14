<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsingSPDataSource.aspx.cs" Inherits="SPCAFContrib.Demo.Layouts.SPCAFContrib.Demo.UsingSPDataSource" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <script>
         $(document).ready(function () {
             $('#zz16_RootAspMenu li.static.selected').removeClass('selected');
         });
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    
    <SharePoint:SPGridView 
    runat="server"  
    ID="grdPropertyValues" 
    AutoGenerateColumns="true" 
    RowStyle-BackColor="#DDDDDD" 
    AlternatingRowStyle-BackColor="#EEEEEE">
    
    </SharePoint:SPGridView> 

    <SharePoint:SPDataSource runat="server" ID="dsPersonTitles" DataSourceMode="List" SelectCommand="<Query><OrderBy><FieldRef Name='{SortField}' Ascending='{SortAsc}' /></OrderBy></Query>">
      <SelectParameters>
          <asp:Parameter Name="WebUrl" DefaultValue="/configuration/" />
          <asp:Parameter Name="ListName" DefaultValue="PersonTitles" />
          <asp:QueryStringParameter Name="SortField" QueryStringField="SortField" DefaultValue="SortOrder" />
          <asp:QueryStringParameter Name="SortAsc" QueryStringField="SortAsc" DefaultValue="true" />
      </SelectParameters>
    </SharePoint:SPDataSource> 

<asp:DropDownList runat="server" ID="ddlPersonTitles" CssClass="title" DataSourceID="dsPersonTitles" DataTextField="Title" DataValueField="ID">
</asp:DropDownList>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
