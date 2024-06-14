using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Microsoft.Office.Server.Search.Query;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.Common;
using SPCAFContrib.Demo.Logging;

namespace SPCAFContrib.Demo.Layouts
{
    public partial class NotifyUser : WebPartPage
    {
        const string CREATOR_FIELD_NAME = "Author";

        const string UPQuery1 = "SELECT  AccountName, FirstName, LastName, WorkEmail " +
                                    "FROM Scope() " +
                                    "WHERE ( (\"SCOPE\" = 'People') )  AND (CONTAINS (FirstName, '{0}') OR CONTAINS (LastName, '{0}') OR CONTAINS (UserName, '{0}'))" +
                                    "ORDER BY \"Rank\" DESC";

        const string UPQuery2 = "SELECT  AccountName, FirstName, LastName, WorkEmail " +
                                    "FROM Scope() " +
                                    "WHERE ( (\"SCOPE\" = 'People') )  AND (CONTAINS (FirstName, '{0}') AND CONTAINS (LastName, '{1}') OR CONTAINS (FirstName, '{1}') AND CONTAINS (LastName, '{0}') )" +
                                    "ORDER BY \"Rank\" DESC";

        private List<MeetingNotificationRecord> sendHistory;

        protected class RecipientInfo
        {
            public int RecipientId { get; set; }
            public string accountName;
            public string FullName {get; set;}
            public string EMail { get; set; }
            public bool Resolved { get; set; }
            public string ResolveInfo { get; set; }
            public bool IsOrganizer { get; set; }

            public RecipientInfo()
            {
                IsOrganizer = false;
                Resolved = false;
            }

            public RecipientInfo(string name)
            {
                this.FullName = name;
            }
        }

        protected class EventInfo
        {
            public string Name;
            public DateTime BeginDate;
            public double Duration;
            public string Customer;
            public string Location;
            public string Description;
            public string Organizer;
            public int OrganizerId;
            public string ExchangeItemId;
        }
        
        protected Guid ListId
        {
            get { try { return new Guid(Request.QueryString["ListId"].ToString()); } catch { return Guid.Empty; } }
        }

        protected bool IsDlg
        {
            get { return Request.QueryString["IsDlg"] != null; }
        }

        protected int ListItemId
        {
            get { return Convert.ToInt32(Request.QueryString["Id"].ToString()); }
        }

        protected SPWeb TargetWeb = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            TargetWeb = SPContext.Current.Web;

            if (!this.IsPostBack && !ListId.Equals(Guid.Empty))
            {
                EventInfo ev = null;

                List<RecipientInfo> recipientList = new List<RecipientInfo>();
                
                SPList list = null;
                try
                {
                    list = TargetWeb.Lists[ListId];                    
                }
                catch(Exception ex)
                {
                    SPSite site = SPContext.Current.Site;

                    foreach (SPWeb innerWeb in site.AllWebs)
                    {
                        try
                        {
                            list = innerWeb.Lists[ListId];
                        }
                        catch 
                        {
                            continue;
                        }

                        if (list != null)
                        {
                            TargetWeb = innerWeb;
                            break;
                        }
                    }

                    if (list == null)
                        ex.LogError(String.Format("Berggren.Intranet.Layouts.Intranet.NotifyUser: List not found {0}", ListId));
                }

                if (list != null)
                {
                    SPListItem item = list.GetItemById(ListItemId);
                                        
                    if (item != null)
                    {
                        ev = GetEventInfo(item);   

                        // check new People and Group field
                        SPFieldUser userField = list.Fields.GetField(getPersonAndGroupListFieldName(list)) as SPFieldUser;
                        if (userField != null)
                        {
                            object currentValue = item[item.Fields[getPersonAndGroupListFieldName(list)].Title];
                            if (currentValue != null)
                            {
                                SPFieldUserValueCollection fieldValues = userField.GetFieldValue(currentValue.ToString()) as SPFieldUserValueCollection;
                                if (fieldValues != null)
                                    foreach (var fieldValue in fieldValues)
                                    {
                                        SPUser user = fieldValue.User;

                                        RecipientInfo recipientInfo = new RecipientInfo();

                                        recipientInfo.FullName = user.Name;
                                        recipientInfo.accountName = user.LoginName;
                                        recipientInfo.EMail = user.Email;
                                        recipientInfo.Resolved = true;
                                        recipientInfo.RecipientId = user.ID;

                                        recipientList.Add(recipientInfo);
                                    }
                            }
                        }

                        // check other field
                        var itemValue = item[getEmailFieldName(list)];
                        if (itemValue != null)
                        {
                            string recipients = itemValue.ToString();
                            if (!String.IsNullOrEmpty(recipients))
                            {
                                string[] recipientArray = recipients.Split(new string[] { ",", ";", "+", "(", ")", "\\", "/", "&", " ja ", " "}, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string rcp in recipientArray)
                                {
                                    string recipient = Regex.Replace(rcp, "[!?æøåÆØÅäöÄÖ]", (m) =>
                                                    (m.Value == "!") ? String.Empty :
                                                    (m.Value == "?") ? String.Empty :
                                                    (m.Value == "æ") ? "e" :
                                                    (m.Value == "ø") ? "o" :
                                                    (m.Value == "å") ? "a" :
                                                    (m.Value == "Æ") ? "e" :
                                                    (m.Value == "Ø") ? "o" :
                                                    (m.Value == "Å") ? "a" :
                                                    (m.Value == "ä") ? "a" :
                                                    (m.Value == "ö") ? "o" :
                                                    (m.Value == "Ä") ? "a" :
                                                    (m.Value == "Ö") ? "o" : m.Value);

                                    if (String.IsNullOrEmpty(recipient)) continue;

                                    RecipientInfo recipientInfo = FindPerson(SPContext.Current.Site.RootWeb, recipient);

                                    if (!recipientInfo.Resolved || String.IsNullOrEmpty(recipientInfo.EMail))
                                    {
                                        IList<RecipientInfo> webUsers = ResoveUsersForWebByName(SPContext.Current.Site.RootWeb, recipient);
                                        if (webUsers.Count > 0)
                                        {
                                            foreach (RecipientInfo webUser in webUsers)
                                            {
                                                if (!String.IsNullOrEmpty(webUser.EMail) && FindRecipientIdx(recipientList, webUser) == -1)
                                                    recipientList.Add(webUser);
                                            }
                                        }
                                        else
                                        {
                                            if (FindRecipientIdx(recipientList, recipientInfo) == -1)
                                            {
                                                // try to check string for email syntax
                                                if (StringHelper.IsEmail(recipient))
                                                {
                                                    recipientInfo.EMail = recipient;
                                                    recipientInfo.Resolved = true;
                                                }
                                                recipientList.Add(recipientInfo); // anyway add this to inform user about unresolved item
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (FindRecipientIdx(recipientList, recipientInfo) == -1)
                                            recipientList.Add(recipientInfo);
                                    }
                                }
                            }
                        }

                        // put organizer to the list
                        RecipientInfo organizerInfo = new RecipientInfo();
                        SPUser organizer = item.GetFieldUser(CREATOR_FIELD_NAME);

                        if (organizer != null)                  
                        {
                            organizerInfo.FullName = organizer.Name;
                            organizerInfo.accountName = organizer.LoginName;
                            organizerInfo.EMail = organizer.Email;
                            organizerInfo.Resolved = true;
                            organizerInfo.RecipientId = organizer.ID;
                            organizerInfo.IsOrganizer = true;

                            ev.Organizer = organizerInfo.accountName;
                            ev.OrganizerId = organizer.ID;
                        }
                        else
                        {
                            organizerInfo = FindPerson(SPContext.Current.Site.RootWeb, ev.Organizer);

                            if (!organizerInfo.Resolved || String.IsNullOrEmpty(organizerInfo.EMail))
                                organizerInfo = ResoveUserForWebById(SPContext.Current.Site.RootWeb, ev.OrganizerId);

                            organizerInfo.IsOrganizer = true;                            
                        }

                        if (organizerInfo.Resolved && !String.IsNullOrEmpty(organizerInfo.EMail))
                        {
                            int Idx = FindRecipientIdx(recipientList, organizerInfo);

                            if (Idx > -1)
                                recipientList[Idx].IsOrganizer = true;
                            else
                                recipientList.Add(organizerInfo);

                            ev.Organizer = organizerInfo.accountName;
                        }
                        else
                            btnSend.Enabled = false;
                        
                    }
                }

                if (recipientList.Count > 0)
                {
                    sendHistory = MeetingNotification.GetMeetingNotification(ListItemId);
                    
                    if (sendHistory.Count > 0)
                        ev.ExchangeItemId = sendHistory[0].ExchangeItemId;
                    
                    SerializeEvent(ev);
                    
                    rptRecepients.DataSource = recipientList;
                    rptRecepients.DataBind();
                }
            }

        }
        
        private int FindRecipientIdx(List<RecipientInfo> recipientList, RecipientInfo recipient)
        {
            int result = -1;
            int i = 0;

            if (!String.IsNullOrEmpty(recipient.EMail))
            {
                foreach (var item in recipientList)
                {
                    if (item.EMail == recipient.EMail)
                    {
                        result = i;
                        break;
                    }

                    i++;
                }
            }

            return result;
        }

        private RecipientInfo ResoveUserForWebById(SPWeb web, int userId)
        {
            RecipientInfo result = new RecipientInfo();
            SPUser user = web.AllUsers.GetByID(userId);
            if (user != null)
            {
                result.FullName = user.Name;
                result.accountName = user.LoginName;
                result.EMail = user.Email;
                result.Resolved = true;
                result.RecipientId = user.ID;
            }

            return result;
        }

        private IList<RecipientInfo> ResoveUsersForWebByName(SPWeb web, string userName)
        {
            List<RecipientInfo> result = new List<RecipientInfo>();
            bool reachedMaxCount = false;
            IList<SPPrincipalInfo> users = SPUtility.SearchPrincipals(web, userName, SPPrincipalType.User, SPPrincipalSource.All, null, 100, out reachedMaxCount);
            if (users.Count > 0)
            {
                foreach (SPPrincipalInfo principalInfo in users)
                {
                    result.Add(new RecipientInfo() { 
                        RecipientId = principalInfo.PrincipalId,
                        FullName = principalInfo.DisplayName,
                        accountName = principalInfo.LoginName,
                        EMail = principalInfo.Email,
                        IsOrganizer = false,
                        Resolved = true
                    });
                }
            }

            return result;
        }

        private EventInfo GetEventInfo(SPListItem item)
        {
            EventInfo result = new EventInfo()            
            {
                Name = CheckField(item.ParentList, getMeetingTitleFieldName(item.ParentList)) && item[getMeetingTitleFieldName(item.ParentList)] != null ? item[getMeetingTitleFieldName(item.ParentList)].ToString() : String.Empty,
                BeginDate = CheckField(item.ParentList, getMeetingStartDateFieldName(item.ParentList)) && item[getMeetingStartDateFieldName(item.ParentList)] != null ? (DateTime)item[getMeetingStartDateFieldName(item.ParentList)] : DateTime.MinValue,
                Customer = CheckField(item.ParentList, getMeetingCustomerFieldName(item.ParentList)) && item[getMeetingCustomerFieldName(item.ParentList)] != null ? item[getMeetingCustomerFieldName(item.ParentList)].ToString() : String.Empty,
                Location = CheckField(item.ParentList, getMeetingLocationFieldName(item.ParentList)) && item[getMeetingLocationFieldName(item.ParentList)] != null ? item[getMeetingLocationFieldName(item.ParentList)].ToString() : String.Empty,
                Description = CheckField(item.ParentList, getMeetingDescriptionFieldName(item.ParentList)) && item[getMeetingDescriptionFieldName(item.ParentList)] != null ? item[getMeetingDescriptionFieldName(item.ParentList)].ToString() : String.Empty,
                Organizer = item[CREATOR_FIELD_NAME].ToString().Split('#')[1],
                OrganizerId = Int32.Parse(item[CREATOR_FIELD_NAME].ToString().Split(';')[0]),
                Duration = CheckField(item.ParentList, getMeetingDurationFieldName(item.ParentList)) && item[getMeetingDurationFieldName(item.ParentList)] != null ? (double)item[getMeetingDurationFieldName(item.ParentList)] : 1.0
            };

            return result;
        }

        private void SerializeEvent(EventInfo ev)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ev.BeginDate = ev.BeginDate.ToUniversalTime();
            hfMeetingTitle.Value = js.Serialize(ev);
        }
        
        private RecipientInfo FindPerson(SPWeb web, string recipient)
        {
            ResultTableCollection resultTables = null;
            RecipientInfo result = new RecipientInfo(recipient) ;

            try
            {
                using (FullTextSqlQuery qRequest = new FullTextSqlQuery(web.Site))
                {
                    string[] names = recipient.Trim().Split(' ');
                    if (names.Length > 1)
                        qRequest.QueryText = String.Format(UPQuery2, names[0], names[1]);
                    else
                        qRequest.QueryText = String.Format(UPQuery1, recipient);
                    qRequest.ResultTypes = ResultType.RelevantResults;
                    qRequest.TrimDuplicates = false;
                    qRequest.RowLimit = 500;

                    resultTables = qRequest.Execute();
                }
            }
            catch (Exception ex)
            {
                result.ResolveInfo = ex.Message;                
            }

            if (resultTables != null && resultTables.Count > 0)
            {
                ResultTable relevantResults = resultTables[ResultType.RelevantResults];

                if (relevantResults.RowCount == 0)
                {
                    result.Resolved = false;
                    result.ResolveInfo = LocalizationHelper.GetResourceString(Consts.SiteResourceFile, "ui_external", "External");
                }
                else if (relevantResults.RowCount > 1)
                {
                    string email_tmp = String.Empty;
                    bool differentUsers = false;

                    foreach (DataRow row in relevantResults.Table.Rows)
                    {
                        if (String.IsNullOrEmpty(row[3].ToString())) continue;

                        if (String.IsNullOrEmpty(email_tmp))
                            email_tmp = row[3].ToString();
                        else if (email_tmp != row[3].ToString())
                            differentUsers = true;
                    }

                    if (differentUsers)
                    {
                        result.Resolved = false;
                        result.ResolveInfo = LocalizationHelper.GetResourceString(Consts.SiteResourceFile, "ui_toomanyusers", "Too many users");
                    }
                    else
                    {
                        DataRow row = relevantResults.Table.Rows[relevantResults.RowCount - 1];

                        result.Resolved = true;
                        result.accountName = row[0].ToString();
                        result.FullName = String.Format("{0} {1}", row[1], row[2]);
                        if (String.IsNullOrEmpty(result.FullName))
                            result.FullName = result.accountName;
                        result.EMail = email_tmp;
                    }
                }
                else
                {
                    DataRow row = relevantResults.Table.Rows[0];

                    result.Resolved = true;
                    result.accountName = row[0].ToString(); 
                    result.FullName = String.Format("{0} {1}", row[1], row[2]);
                    if (String.IsNullOrEmpty(result.FullName))
                        result.FullName = result.accountName;
                    result.EMail = row[3].ToString();
                }
            }

            return result;
        }

        private string getMeetingDescriptionFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Huomioitavaa";
                default:
                    return String.Empty;
            }
        }

        private string getMeetingLocationFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Sijainti";
                default:
                    return String.Empty;
            }
        }

        private string getMeetingCustomerFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Asiakas";
                default:
                    return String.Empty;
            }
        }

        private string getMeetingStartDateFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Ajankohta";
                default:
                    return String.Empty;
            }
        }        

        private string getMeetingDurationFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Kesto";
                default:
                    return String.Empty;
            }
        }

        private string getMeetingTitleFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Asiakas";
                default:
                    return String.Empty;
            }
        }              

        private string getEmailFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Osallistujat";
                default:
                    return String.Empty;
            }
        }

        private string getPersonAndGroupListFieldName(SPList list)
        {
            switch (list.Title)
            {
                case "Berggren asiakastapaamiset":
                    return "Berggrenin osallistujat";
                default:
                    return String.Empty;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (IsDlg)
            {
                btnClose.Attributes.Add("onclick", "window.frameElement.cancelPopUp();return false;");
                btnClose.Attributes.Add("target", "_self");
            }
        }

        protected void rptRecepients_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RecipientInfo item = (RecipientInfo)e.Item.DataItem;
                Label ltrName = e.Item.FindControl("ltrName") as Label;
                Label ltrEMail = e.Item.FindControl("ltrEMail") as Label;
                Label lblResolveInfo = e.Item.FindControl("lblResolveInfo") as Label;
                CheckBox cbSelected = e.Item.FindControl("cbSelected") as CheckBox;
                System.Web.UI.WebControls.Image imgSendStatus = e.Item.FindControl("imgSendStatus") as System.Web.UI.WebControls.Image;

                imgSendStatus.ImageUrl = "";
                imgSendStatus.ToolTip = "";
                imgSendStatus.Visible = false;

                if (!item.Resolved)
                {
                    lblResolveInfo.ForeColor = Color.Red;
                    cbSelected.Enabled = false;
                }

                if (String.IsNullOrEmpty(item.EMail))
                {
                    cbSelected.Checked = false;
                    cbSelected.Enabled = false;
                }

                if (item.IsOrganizer)
                {
                    ltrName.Font.Bold = true;
                    ltrEMail.Font.Bold = true;
                    lblResolveInfo.Font.Bold = true;
                    lblResolveInfo.Text = LocalizationHelper.GetResourceString(Consts.SiteResourceFile, "ui_organizer", "organizer");
                }

                int Idx = MeetingNotification.GetRecordIndexByEmail(sendHistory, item.EMail);
                if (Idx >= 0)
                {                    
                    imgSendStatus.ImageUrl = TargetWeb.Url + "/_layouts/images/itgbcirc.png";
                    imgSendStatus.ToolTip = sendHistory[Idx].SendDate.ToString();                    
                    if (sendHistory[Idx].SendStatus == (int)MeetingNotificationSendStatus.Failed)
                    {
                        imgSendStatus.ImageUrl = TargetWeb.Url + "/_layouts/images/WARN16.GIF";
                        imgSendStatus.ToolTip += "\r\n" + sendHistory[Idx].Message;
                    }
                    else
                        cbSelected.Checked = false;

                    imgSendStatus.Visible = true;
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            List<string> participantEmails = new List<string>();
            List<int> participantIds = new List<int>();
            List<string> notSentParticipantEmails = new List<string>();
            List<string> notSentErrors = new List<string>();
            List<string> dbAccessFailedParticipantEmails = new List<string>();
            List<string> dbAccessFailedErrors = new List<string>();
            string organizerEMail = String.Empty;            

            // prepare participats list
            foreach (RepeaterItem item in rptRecepients.Items)
            {
                CheckBox cbSelected = item.FindControl("cbSelected") as CheckBox;
                Label ltrEMail = item.FindControl("ltrEMail") as Label;
                HiddenField hfIsOrganizer = item.FindControl("hfIsOrganizer") as HiddenField;
                HiddenField hfUserId = item.FindControl("hfUserId") as HiddenField;

                if (cbSelected.Checked && !participantEmails.Contains(ltrEMail.Text))
                {
                    participantEmails.Add(ltrEMail.Text);
                    int tmp_userId;
                    if (Int32.TryParse(hfUserId.Value, out tmp_userId))
                        participantIds.Add(tmp_userId);
                    else
                        participantIds.Add(0);
                }

                if (Convert.ToBoolean(hfIsOrganizer.Value))
                {
                    organizerEMail = ltrEMail.Text;
                }
            }            

            if (!String.IsNullOrEmpty(organizerEMail))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                EventInfo ev = js.Deserialize<EventInfo>(hfMeetingTitle.Value);
                ev.BeginDate = ev.BeginDate.ToLocalTime();

                int i = 0;
                string error_message = String.Empty;
                MeetingNotificationSendStatus sendstatus = MeetingNotificationSendStatus.Success;

                #region Exchange
                // Exchange server will send notification for all participants. We do not need to loot over each email
                string calendarItemId = ev.ExchangeItemId;

                if (!MailHelper.SendExchangeMeetingRequest(organizerEMail, ev.Name, tbMessage.Text + "\r\n" + ev.Description, ev.BeginDate, ev.Duration, ev.Location, participantEmails.ToArray(), ListItemId, ref calendarItemId))
                {
                    notSentParticipantEmails.AddRange(participantEmails);
                    foreach (var item in participantEmails)
                        notSentErrors.Add(MailHelper.LastError); // each participant will have same error message

                    sendstatus = MeetingNotificationSendStatus.Failed;
                    error_message = MailHelper.LastError;
                }

                foreach (string emailTo in participantEmails)
                {
                    // put send status to custom DB
                    try
                    {
                        MeetingNotification.AddMeetingNotification(ListItemId, participantIds[i], sendstatus, error_message, TargetWeb.CurrentUser.ID, emailTo, organizerEMail, calendarItemId);
                    }
                    catch (Exception ex)
                    {
                        dbAccessFailedParticipantEmails.Add(emailTo);
                        dbAccessFailedErrors.Add(ex.Message);
                        ex.LogError("Put send status to custom DB");
                    }

                    i++;
                } 
                #endregion

                #region iCal
                /* DVD: To send over SMTP server and iCal format
                foreach (string emailTo in participantEmails)
                {
                    sendstatus = MeetingNotificationSendStatus.Success;    
                    error_message = String.Empty;

                    if (!MailHelper.SendMeetingRequest(emailTo, ev.Name, tbMessage.Text + "\r\n" + ev.Description, ev.BeginDate, ev.Duration, ev.Location, organizerEMail, participantEmails.ToArray()))
                    {
                        notSentParticipantEmails.Add(emailTo);
                        notSentErrors.Add(MailHelper.LastError);
                        sendstatus = MeetingNotificationSendStatus.Failed;
                        error_message = MailHelper.LastError;
                    }
                    
                    // put send status to custom DB
                    try
                    {
                        MeetingNotification.AddMeetingNotification(ListItemId, participantIds[i], sendstatus, error_message, TargetWeb.CurrentUser.ID, emailTo, organizerEMail);
                    }
                    catch (Exception ex)
                    {
                        dbAccessFailedParticipantEmails.Add(emailTo);
                        dbAccessFailedErrors.Add(ex.Message);
                        Logger.Log("Put send status to custom DB", ex);
                    }                    

                    i++;
                }
                */

                #endregion

                if (notSentParticipantEmails.Count + dbAccessFailedParticipantEmails.Count > 0)
                {
                    SPWeb web = SPContext.Current.Web;
                    // set send status for recipients
                    foreach (RepeaterItem item in rptRecepients.Items)
                    {
                        CheckBox cbSelected = item.FindControl("cbSelected") as CheckBox;
                        Label ltrEMail = item.FindControl("ltrEMail") as Label;
                        System.Web.UI.WebControls.Image imgSendStatus = item.FindControl("imgSendStatus") as System.Web.UI.WebControls.Image;

                        imgSendStatus.ImageUrl = "";
                        imgSendStatus.ToolTip = "";
                        imgSendStatus.Visible = false;

                        if (cbSelected.Checked && !String.IsNullOrEmpty(ltrEMail.Text))
                        {
                            if (notSentParticipantEmails.Contains(ltrEMail.Text))
                            {
                                int Idx = notSentParticipantEmails.IndexOf(ltrEMail.Text);
                                imgSendStatus.ImageUrl = web.Url + "/_layouts/images/EXCLAIM.GIF";
                                imgSendStatus.AlternateText = LocalizationHelper.GetResourceString(Consts.SiteResourceFile, "ui_notsent", "Not sent");
                                imgSendStatus.ToolTip = notSentErrors[Idx];
                            }
                            else if (dbAccessFailedParticipantEmails.Contains(ltrEMail.Text))
                            {
                                int Idx = dbAccessFailedParticipantEmails.IndexOf(ltrEMail.Text);
                                imgSendStatus.ImageUrl = web.Url + "/_layouts/images/WARN16.GIF";
                                imgSendStatus.AlternateText = LocalizationHelper.GetResourceString(Consts.SiteResourceFile, "ui_notsavedtodb", "Not saved to DB");
                                imgSendStatus.ToolTip = dbAccessFailedErrors[Idx];
                            }
                            else
                            {
                                imgSendStatus.ImageUrl = web.Url + "/_layouts/images/PpsMaAdminValid.png";
                                imgSendStatus.AlternateText = LocalizationHelper.GetResourceString(Consts.SiteResourceFile, "ui_sent_ok", "Send success");
                            }
                            imgSendStatus.Visible = true;
                        }
                    }
                }
                else // close dialog in case all send efforts were OK                
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "closeDialog_key", "window.frameElement.cancelPopUp(); return false;", true);                    
                }
            }
        }

        private bool CheckField(SPList list, string fieldName)
        {
            string url = SPContext.Current.Site.Url;
            return list.Fields.ContainsField(fieldName);
        }
    }
}
