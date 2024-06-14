using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.Logging;

namespace SPCAFContrib.Demo.Common
{
    public static class MeetingNotification
    {        
        const string ADDNOTIFICATION_PROC_NAME = "AddNotification";
        const string GETMEETINGNOTIFICATIONS_PROC_NAME = "GetMeetingNotifications";

        public static void AddMeetingNotification(int meetingId, int userId, MeetingNotificationSendStatus status, string messageText, int senderId, string toEmail, string fromEmail, string exchangeItemId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate() // to run under AppPool priv
            {
                if (!String.IsNullOrEmpty(Intra10DB.ConnectionString))
                {
                    using (SqlConnection con = new SqlConnection(Intra10DB.ConnectionString))
                    {
                        SqlCommand command = new SqlCommand(ADDNOTIFICATION_PROC_NAME, con);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@MeetingId", meetingId));
                        if (userId > 0)
                            command.Parameters.Add(new SqlParameter("@UserId", userId));
                        command.Parameters.Add(new SqlParameter("@SendStatus", (int)status));
                        if (!String.IsNullOrEmpty(messageText))
                            command.Parameters.Add(new SqlParameter("@Message", messageText));
                        command.Parameters.Add(new SqlParameter("@SenderId", senderId));
                        if (!String.IsNullOrEmpty(toEmail))
                            command.Parameters.Add(new SqlParameter("@ToEmail", toEmail));
                        if (!String.IsNullOrEmpty(fromEmail))
                            command.Parameters.Add(new SqlParameter("@FromEmail", fromEmail));
                        if (!String.IsNullOrEmpty(exchangeItemId))
                            command.Parameters.Add(new SqlParameter("@ExchangeItemId", exchangeItemId));
                        try
                        {
                            con.Open();
                            command.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            ex.LogError("AddMeetingNotification");
                        }
                    }
                }
                else
                    Logger.Instance.LogWarning("Unspecified Intra10 DB connection string");
            });            
        }

        public static List<MeetingNotificationRecord> GetMeetingNotification(int meetingId)
        {
            List<MeetingNotificationRecord> result = new List<MeetingNotificationRecord>();
            SPSecurity.RunWithElevatedPrivileges(delegate() // to run under AppPool priv
            {
                if (!String.IsNullOrEmpty(Intra10DB.ConnectionString))
                {
                    using (SqlConnection con = new SqlConnection(Intra10DB.ConnectionString))
                    {
                        SqlCommand command = new SqlCommand(GETMEETINGNOTIFICATIONS_PROC_NAME, con);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@MeetingId", meetingId));

                        try
                        {
                            con.Open();
                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    result.Add(new MeetingNotificationRecord()
                                    {
                                        MeetingId = Convert.ToInt32(reader[0]),
                                        ExchangeItemId = reader[1].ToString(),
                                        UserId = Convert.IsDBNull(reader[2]) ? 0 : Convert.ToInt32(reader[2]),
                                        SendDate = Convert.ToDateTime(reader[3]),
                                        SendStatus = Convert.ToByte(reader[4]),
                                        Message = reader[5].ToString(),
                                        SenderId = Convert.ToInt32(reader[6]),
                                        ToEmail = reader[7].ToString(),
                                        FromEmail = reader[8].ToString()                                        
                                    });
                                }
                            }

                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            ex.LogError("GetMeetingNotification");
                        }
                    }
                }
                else
                    Logger.Instance.LogWarning("Unspecified Intra10 DB connection string");
            });

            return result;
        }

        public static int GetRecordIndexByEmail(List<MeetingNotificationRecord> list, string email)
        {
            int i = 0;
            foreach (MeetingNotificationRecord record in list)
            {
                if (record.ToEmail == email)
                    return i;
                i++;
            }

            return -1;
        }
    
    }

    public enum MeetingNotificationSendStatus
    {
        Success = 1,
        Failed = 0
    }

    public struct MeetingNotificationRecord 
    {
        public int MeetingId;
        public string ExchangeItemId;
        public int UserId;
        public DateTime SendDate;
        public byte SendStatus;
        public string Message;
        public int SenderId;
        public string ToEmail;
        public string FromEmail;        
    }
}
