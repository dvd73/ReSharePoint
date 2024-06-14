using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.SharePoint;
using System.Net;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.Logging;

namespace SPCAFContrib.Demo.Common
{
	public class MailHelper
	{
		public static string LastError;
		public static bool SendMeetingRequest(string toAddress, string title, string body, DateTime startDate, double duration, string location, string fromAddress, string[] attendees)
		{
			//init the message with your defaults (from, to, subject, etc)
			MailMessage message = initMailMessage(toAddress, title, body);
			string iCal = initICal(title, body, startDate, duration, location, fromAddress, true, String.Empty, false, 0, 0, attendees);            
			
			System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
			ct.Parameters.Add("method", "REQUEST");
			AlternateView avCal = AlternateView.CreateAlternateViewFromString(iCal, ct);
			message.AlternateViews.Add(avCal);

			//Alternative way
			/*
			System.Net.Mail.Attachment attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(iCal, new ContentType("text/calendar; method=request; charset=UTF-8; component=vevent"));
			attachment.TransferEncoding = TransferEncoding.Base64;
			attachment.Name = "EventDetails.ics"; //not visible in outlook            

			message.Attachments.Add(attachment); */            
			
			try
			{
				sendMailMessage(message);
				return true;
			}
			catch (Exception ex)
			{
				ex.LogError("SPCAFContrib.Demo.Common.MailHelper");
				LastError = createMessage(ex);
				return false;
			}
		}
        public static bool SendExchangeMeetingRequest(string organizerEMail, string title, string body, DateTime startDate, double duration, string location, string[] attendees, int listItemId, ref string itemId)
        {
            return true;
        }

		private static MailMessage initMailMessage(string toAddress, string subject, string body)
		{
			MailAddress maFrom = new MailAddress(GetFromEmailID());  // this email should not be as organizer          

			MailMessage result = new MailMessage();
			result.From = maFrom;
			result.Subject = subject;
			result.Body = body;

			result.To.Add(toAddress);

			return result;
		}

		private static void sendMailMessage(MailMessage mailMessage)
		{
			SPSecurity.RunWithElevatedPrivileges(delegate() // to run under AppPool priv
			{
				string mailHost = GetSMTPHostName();
				SmtpClient smtpClient = new SmtpClient(mailHost, 25);
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				//smtpClient.UseDefaultCredentials = true;
				smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
				//smtpClient.Credentials = new NetworkCredential("sharepointservice", "ser234vice", "BERGGREN-YHTIÖT");
				smtpClient.Send(mailMessage);
			});
		}

		private static string initICal(string title, string body, DateTime startDate,
		   double duration, string location, string organizer,
		   bool updatePreviousEvent, string eventId,
		   bool allDayEvent,
		   int recurrenceDaysInterval, int recurrenceCount,
		   string[] attendees)
		{
		    return "";
		}

		public static string GetSMTPHostName()
		{
			Guid siteID = SPContext.Current.Site.ID;

			using (SPSite site = new SPSite(siteID))
			{
				//Get the SMTP host name from "Outgoing e-mail settings"
				return site.WebApplication.OutboundMailServiceInstance.Parent.Name;
			}
		}

		public static string GetFromEmailID()
		{
			Guid siteID = SPContext.Current.Site.ID;

			using (SPSite site = new SPSite(siteID))
			{
				//Get the "from email address" from "Outgoing e-mail settings"
				return site.WebApplication.OutboundMailSenderAddress;
			}
		}

		private static string createMessage(Exception ex)
		{
			StringBuilder builder = new StringBuilder();
			Exception innerException = ex;
			do
			{
				builder.Append(innerException.GetType().ToString() + ": ").Append(innerException.Message).Append("; ");
				innerException = innerException.InnerException;
			}
			while (innerException != null);
			return builder.ToString();
		}
	}    
}
