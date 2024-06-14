using System;
using System.Drawing;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using System.Net.Mail;
using System.Web;
using System.Net;
using System.Net.Mime;

namespace SPCAFContrib.Demo.ControlTemplates
{
    [Description("Обращения граждан")]
    public partial class FeedbackUserControl : UserControl
    {
        private const string SCRIPT_DOFOCUS =
        @"window.setTimeout('DoFocus()', 1);
        function DoFocus()
        {
            try {
                document.getElementById('REQUEST_LASTFOCUS').focus();
            } catch (ex) {}
        }";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                HookOnFocus(this.Page as Control);

            ScriptManager.RegisterStartupScript(
                this,
                typeof(FeedbackUserControl),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                try
                {
                    SPSite site = SPContext.Current.Site;
                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        //using (SPSite site1 = new SPSite(ConfigurationManager.AppSettings["QuestionsListWebUrl"]))                        
                        using (SPWeb web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            SPList destination = web.Lists["Обращения граждан"];
                            SPListItem item = destination.Items.Add();
                            item["Ф.И.О."] = txtName.Text;
                            item["Почтовый адрес"] = txtAddress.Text;
                            item["Ваш e-mail"] = txtEmail.Text;
                            item["Текст обращения"] = txtQuestion.Text;
                            if (SPContext.Current.Web.CurrentUser != null)
                                item["Кем создано"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                    );
                    pnlData.Visible = false;
                    pnlResult.Visible = true;
                }
                catch (Exception ex)
                {
                    SPUtility.TransferToErrorPage(ex.Message);
                }

                String FIO, Address, email, Question, CreateTime;
                String MessageText, MessageSubject;

                FIO = "Ф.И.О.: " + txtName.Text + "\n";
                Address = "Почтовый адрес: " + txtAddress.Text + "\n";
                email = "e-mail: " + txtEmail.Text + "\n";
                Question = "Текст обращения: " + txtQuestion.Text + "\n";
                CreateTime = "Дата отправки обращения: " + DateTime.Now.ToString() + "\n";

                MessageText = FIO + Address + email + Question + CreateTime;
                MessageSubject = "Заголовок";

                try
                {
                    SendMail(MessageSubject, MessageText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }
            }
        }
        
        protected bool ValidateCaptcha()
        {
            bool captchaValid = false;

            lblResult.Visible = true;
            if (Session["ASPCAPTCHA"] == null || String.IsNullOrEmpty(Session["ASPCAPTCHA"].ToString()))
            {
                lblResult.Text = "Текст просрочен, попробуйте заново, предварительно обновив";
                lblResult.ForeColor = Color.Red;
            }
            else
            {
                String TestValue = txtCaptcha.Text.Trim().ToUpper(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                if (String.Compare(TestValue, Session["ASPCAPTCHA"].ToString().Trim()) == 0)
                {
                    lblResult.Text = "CAPTCHA PASSED";
                    lblResult.ForeColor = Color.Green;
                    captchaValid = true;
                }
                else
                {
                    lblResult.Text = "Текст с картинки введен не правильно";
                    lblResult.ForeColor = Color.Red;
                }

                //IMPORTANT: You must remove session value for security after the CAPTCHA test//
                Session.Remove("ASPCAPTCHA");
                //////////
            }

            return captchaValid;
        }


        protected void btnRefreh_Click(Object sender, EventArgs e)
        {
            lblResult.Visible = false;
            lblResult.Text = "";
            txtCaptcha.Text = "";
        }

        private void SendMail(String MessageSubject, String MessageBody)
        {
            String SmtpClientName = ConfigurationManager.AppSettings["SmtpClient"];
            SmtpClient Smtp = new SmtpClient(SmtpClientName, 25);

            MailMessage Message = new MailMessage();

            String MessageFrom = ConfigurationManager.AppSettings["MessageFrom"];
            Message.From = new MailAddress(MessageFrom);

            String MessageToStr = ConfigurationManager.AppSettings["MessageTo"];

            char[] charSeparators = new char[] { ';' };
            String[] MessageTo = MessageToStr.Split(charSeparators);
            foreach (String mail in MessageTo)
            {
                Message.To.Add(new MailAddress(mail.Trim()));
            }

            Message.Subject = MessageSubject;
            Message.Body = MessageBody;
            Smtp.Send(Message);
        }

        private void HookOnFocus(Control CurrentControl)
        {
            if ((CurrentControl is TextBox) || (CurrentControl is Button))
            {
                (CurrentControl as WebControl).Attributes.Add(
                    "onfocus",
                    "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
            }

            if (CurrentControl.HasControls())
            {
                foreach (Control CurrentChildControl in CurrentControl.Controls)
                {
                    HookOnFocus(CurrentChildControl);
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/appeals/default.aspx");
        }
    }
}
