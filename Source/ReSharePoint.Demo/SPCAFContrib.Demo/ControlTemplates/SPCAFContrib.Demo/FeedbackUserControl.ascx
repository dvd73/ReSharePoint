<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedbackUserControl.ascx.cs" Inherits="SPCAFContrib.Demo.ControlTemplates.FeedbackUserControl" %>

<div style="display:none">
    <style type="text/css">
        .TD_MANDATORY
        {
            width:10px;
            color:Red;
            vertical-align: top;
        }
        .TD_DATA
        {
            width:400px;
        }
        .TD_TITLE
        {
            width:150px;
            vertical-align:top;
        }
        .BUTTON
        {
            width:80px;
        }
        .TEXTBOX
        {
            font-family:Arial;
        }
        table.feedback_table td
        {
            padding:2px
        }
    </style>
</div>
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdatePanel ID="upFeedback" runat="server">
    <ContentTemplate>
        <br />
        <asp:Panel runat="server" ID="pnlData">
            <table class="regular_table feedback_table">
                <tr>
                    <td class="TD_MANDATORY">*</td>
                    <td class="TD_TITLE">Ф.И.О.</td>
                    <td class="TD_DATA">
                        <asp:TextBox runat="server" ID="txtName" Width="100%" CssClass="TEXTBOX" />
                        <asp:RequiredFieldValidator ID="rftbName" runat="server" ControlToValidate="txtName" ErrorMessage="Поле обязательно для заполнения" InitialValue="" Display="Dynamic" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td class="TD_MANDATORY">*</td>
                    <td class="TD_TITLE">Почтовый адрес</td>
                    <td class="TD_DATA">
                        <asp:TextBox runat="server" ID="txtAddress" Width="100%" CssClass="TEXTBOX" TextMode="MultiLine" Height="50px" />
                        <asp:RequiredFieldValidator ID="rftbAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Поле обязательно для заполнения" InitialValue="" Display="Dynamic" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td class="TD_MANDATORY">*</td>
                    <td class="TD_TITLE">Ваш e-mail</td>
                    <td class="TD_DATA">
                        <asp:TextBox runat="server" ID="txtEmail" Width="100%" CssClass="TEXTBOX" />                        
                        <asp:RequiredFieldValidator ID="rftbEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Поле обязательно для заполнения" InitialValue="" Display="Dynamic" ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="regExpEMail" runat="server" ControlToValidate="txtEmail"
                             ErrorMessage="Неверно указан e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                             Display="Dynamic" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td class="TD_MANDATORY">*</td>
                    <td class="TD_TITLE">Текст обращения</td>
                    <td class="TD_DATA">
                        <asp:TextBox runat="server" ID="txtQuestion" Width="100%" Height="150px" 
                            TextMode="MultiLine" CssClass="TEXTBOX"/>
                        <asp:RequiredFieldValidator ID="rftbQuestion" runat="server" ControlToValidate="txtQuestion" ErrorMessage="Поле обязательно для заполнения" InitialValue="" Display="Dynamic" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:right;padding-top:5px;">
                        <img src="/_layouts/Captcha/captcha.ashx" id="img1" /><asp:LinkButton ID="LinkButton1" Text="Обновить" OnClick="btnRefreh_Click" runat="server" />                
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:right;padding-top:5px;padding-bottom:5px;">
                       <asp:TextBox ID="txtCaptcha" runat="server" />               
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:right;padding-bottom:5px">
                       <asp:Label ID="lblResult" Text="" runat="server" Font-Bold="true" Visible="false" />              
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:right;padding-top:10px;">
                        <asp:Button ID="btnOK" runat="server" Text="ОК" CssClass="BUTTON" 
                            onclick="btnOK_Click" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Отмена" CssClass="BUTTON" 
                            onclick="btnCancel_Click"/>                    
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlResult" Visible="false">
            Ваш обращение отправлено.
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>