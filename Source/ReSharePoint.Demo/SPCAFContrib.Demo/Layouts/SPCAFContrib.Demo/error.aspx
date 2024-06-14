<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SPCAFContrib.Demo.Layouts.SPCAFContrib.Demo.Error" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Error page</title>
    
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.8.0/themes/smoothness/jquery-ui.css">

    <script src="http://code.jquery.com/jquery-1.8.0.min.js"></script>
    <script src="http://code.jquery.com/ui/1.8.0/jquery-ui.min.js"></script>
    <script>
        $('.grid').hide();
        $(document).ready(function () {
            $("td.ms-vb2:contains('01.01.1900')").text("");
        });
    </script>
</head>
<body>
    <form id="Form1" runat="server">
        <div class="container_error">
            <div id="masthead">
                <div id="logo">
                    <a id="A1" href="<% $SPUrl:~SiteCollection/%>" title="Go to site home page" runat="server">
                        <img id="Img1" src="<% $SPUrl: ~sitecollection/_layouts/images/MOSS.Common/error.gif%>" runat="server" alt="Error" />
                    </a>
                </div>
            </div>
            <div class="grid">
                <h1>
                    Error</h1>
                <p>
                    We apologize, but an error occurred and your request could not be completed.</p>
                <p>
                    This error has been logged. If you have additional information that you believe
                    may have caused this error, please contact technical support.</p>
            </div>            
        </div>
    </form>
</body>
</html>    