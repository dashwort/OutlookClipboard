using EmailMemoryClass.Configuration;
using EmailMemoryClass.Services;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Outlook = Microsoft.Office.Interop.Outlook;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;

namespace EmailMemoryClass.outlookSearch
{
    public class SearchResult
    {
        public string ConversationIndex { get; set; }
        public string ConversationID { get; set; }
        public string SRNumber { get; set; }
        public string Subject { get; set; }
        public string Account { get; set; }
        public int HasSRNumber { get; set; }
        public string EmailTo { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public DateTime Time { get; set; }
        public string Body { get; set; }

        public string UKTime
        {
            get { return "";}
        }
        public SearchResult()
        {

        }

        public SearchResult(Outlook.MailItem mailItem)
        {
            ConversationIndex = mailItem.ConversationIndex;
            ConversationID = mailItem.ConversationID;
            Time = mailItem.SentOn;
            Subject = mailItem.Subject;
            SRNumber = ExtractSR(mailItem);
            EmailTo = mailItem.To;
            Cc = mailItem.CC;
            Bcc = mailItem.BCC;
            Account = mailItem.SenderName;
            Body = mailItem.Body;
        }

        string ExtractSR(Outlook.MailItem mailItem)
        {
            HasSRNumber = 0;
            string extractedText = "No SR detected";
            string regexPattern = @"(?<!\d)\d{10}(?!\d)";
            Regex regex = new Regex(regexPattern);

            try
            {
                if (!string.IsNullOrEmpty(mailItem.Subject))
                {
                    var text = regex.Match(mailItem.Subject).Value;

                    if (text.Trim().StartsWith("810") || text.Trim().StartsWith("600"))
                        extractedText = text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(mailItem.Body))
                    {
                        var text = regex.Match(mailItem.Body).Value;

                        if (text.Trim().StartsWith("810") || text.Trim().StartsWith("600"))
                            extractedText = text;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Error extract SR: {ex.Message} {ex.InnerException} {ex.StackTrace}", "Error");
            }

            if (extractedText != "No SR detected" && extractedText.StartsWith("810"))
                HasSRNumber = 1;

            return extractedText;
        }
    }
}
