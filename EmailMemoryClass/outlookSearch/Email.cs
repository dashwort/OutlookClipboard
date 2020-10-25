using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;

namespace EmailMemoryClass 
{
    public class Email
    {
        #region fields
        // email related fields
        private string from;
        private string to;
        private string cc;
        private string bcc;
        private string subject;
        private string attachments;
        private string votingoptions;
        private string importance;
        private string signature;
        private string readreceipt;
        private string body;
        private string timesent;

        // functional related fields
        private bool skipped;
        private bool hasedited;
        private bool tosend;
        private bool validationpass;
        private bool reviewed;

        private string conversationIndex;
        private string conversationId;

        // metadata private fields
        private string _srNumber;
        private string _lastMailAsFwd;

        // objects
        private List<string> explicitAttachments;

        #endregion
        public Email(Outlook.MailItem mailItem)
        {
            if (mailItem != null)
            {
                // pulled from mail item
                this.from = mailItem.SenderEmailAddress;
                this.to = mailItem.To;
                this.cc = mailItem.CC;
                this.bcc = mailItem.BCC;
                this.subject = mailItem.Subject;
                this.body = mailItem.HTMLBody;
                this.conversationIndex = mailItem.ConversationIndex;
                this.conversationId = mailItem.ConversationID;
                this.timesent = mailItem.SentOn.ToString();

                // extract using methods
                this._srNumber = ExtractSR();
                this._lastMailAsFwd = GetFwdBody(mailItem);

            }
        }

        #region constructors
        public Email()
        {

        }
        #endregion

        #region properties
        // meta data 
        public string SRNumber
        {
            get { return _srNumber; }
            set
            {
                _srNumber = value;
            }
        }

        public string ConversationId
        {
            get { return conversationId; }
            set { conversationId = value; }
        }

        public string ConversationIndex
        {
            get { return conversationIndex; }
            set { conversationIndex = value; }
        }

        public string Icon
        {
            get { return @"C:\temp\email-icon.png"; }
        }

        public string LastMailAsFwd
        {
            get { return _lastMailAsFwd; }
            set
            {
                _lastMailAsFwd = value;
            }
        }

        public string From
        {
            get { return from; }

            set
            {
                if (value is string)
                {
                    from = value;
                }
                else
                {
                    from = string.Empty;
                }
            }
        }

        public string To
        {
            get { return to; }

            set
            {
                if (value is string)
                {
                    to = value;
                }
                else
                {
                    to = string.Empty;
                }
            }
        }

        public string CC
        {
            get { return cc; }

            set
            {
                if (value is string)
                {
                    cc = value;
                }
                else
                {
                    cc = string.Empty;
                }
            }
        }

        public string BCC
        {
            get { return bcc; }

            set
            {
                if (value is string)
                {
                    bcc = value;
                }
                else
                {
                    bcc = string.Empty;
                }
            }
        }

        public string Subject
        {
            get { return subject; }

            set
            {
                if (value is string)
                {
                    subject = value;
                }
                else
                {
                    subject = string.Empty;
                }
            }
        }

        public string Attachments
        {
            get { return attachments; }

            set
            {
                if (value is string)
                {
                    attachments = value;
                }
                else
                {
                    attachments = string.Empty;
                }
            }
        }

        public string VotingOptions
        {
            get { return votingoptions; }

            set
            {
                if (value is string)
                {
                    if (value == "Yes.No" || value == "Accept.Reject")
                    {
                        votingoptions = value;
                    }
                    else
                    {
                        votingoptions = "None";
                    }
                }
                else
                {
                    votingoptions = "None";
                }
            }
        }

        public string Importance
        {
            get { return importance; }

            set
            {
                if (value is string)
                {
                    if (value == "High" || value == "Low")
                    {
                        importance = value;
                    }
                    else
                    {
                        importance = string.Empty;
                    }
                }
                else
                {
                    importance = string.Empty;
                }
            }
        }

        public string Signature
        {
            get { return signature; }

            set
            {
                if (value is string)
                {
                    if (value == "Anonymous" || value == "Personal" || value == "None")
                    {
                        signature = value;
                    }
                    else
                    {
                        signature = "None";
                    }
                }
                else
                {
                    signature = "None";
                }
            }
        }

        public string ReadReceipt
        {
            get { return readreceipt; }

            set
            {
 
                if (value is string)
                {
                    if (value == "Yes" || value == "No")
                    {
                        readreceipt = value;
                    }
                    else
                    {
                        readreceipt = "No";
                    }
                }
                else
                {
                    readreceipt = "No";
                }
            }
        }

        public string Body
        {
            get { return body; }

            set
            {
                if (value is string)
                {
                    body = value;
                }
                else
                {
                    body = string.Empty;
                }
            }
        }

        public bool Skipped
        {
            get { return skipped; }

            set
            {
                if (value)
                {
                    skipped = value;
                }
                else
                {
                    skipped = false;
                }
            }
        }

        public bool HasEdited
        {
            get { return hasedited; }

            set
            {
                if (value)
                {
                    hasedited = value;
                }
                else
                {
                    hasedited = false;
                }
            }
        }

        public bool ToSend
        {
            get { return tosend; }

            set
            {
                if (value)
                {
                    tosend = value;
                }
                else
                {
                    tosend = true;
                }
            }
        }

        public string TimeSent
        {
            get { return timesent; }
            set { timesent = value; }
        }


        public string BodyText
        {
            get; set;
        }

        #endregion

        #region Methods
        string ExtractSR()
        {
            string extractedText = "No SR detected";
            string regexPattern = @"(?<!\d)\d{10}(?!\d)";
            Regex regex = new Regex(regexPattern);

            try
            {
                if (!string.IsNullOrEmpty(regex.Match(this.subject).Value))
                {
                    var text = regex.Match(this.subject).Value;

                    if (text.Trim().StartsWith("810") || text.Trim().StartsWith("600"))
                        extractedText = text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(regex.Match(this.body).Value))
                    {
                        var text = regex.Match(this.body).Value;

                        if (text.Trim().StartsWith("810") || text.Trim().StartsWith("600"))
                            extractedText = text;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return extractedText;
        }

        string GetFwdBody(Outlook.MailItem mailItem)
        {

            var builder = new StringBuilder();

            try
            {
                var lastEmail = ExtractLastEmail(mailItem.Body);
                builder.AppendLine($"From: {mailItem.SenderName}");
                builder.AppendLine($"Sent: {mailItem.SentOn}");
                builder.AppendLine($"To: {mailItem.To}");
                builder.AppendLine($"Subject: FW:{mailItem.Subject}");
                var removedExtraSpaceFromBody = lastEmail.Replace("\r\n\r\n", "\r\n");
                builder.AppendLine(removedExtraSpaceFromBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting info {ex.Message}");
            }

            return builder.ToString();
        }

        string ExtractLastEmail(string input)
        {
            string[] mailInList = input.Split(new string[] { "From:" }, StringSplitOptions.None);

            if (mailInList.Length == 0)
                return input;
            else
                return mailInList[0];
        }

        public void OpenInOutlook()
        {
            OutlookApp app = new OutlookApp();
            Outlook.MailItem mailitem = app.CreateItem(Outlook.OlItemType.olMailItem);
            mailitem.Subject = this.subject;
            mailitem.HTMLBody = this.body;
            mailitem.To = this.to;
            mailitem.CC = this.cc;
            mailitem.BCC = this.bcc;
            mailitem.ReadReceiptRequested = this.readreceipt == "Yes" ? true : false;

            switch (importance)
            {
                case "High":
                    mailitem.Importance = Outlook.OlImportance.olImportanceHigh;
                    break;
                case "Low":
                    mailitem.Importance = Outlook.OlImportance.olImportanceLow;
                    break;
                default:
                    mailitem.Importance = Outlook.OlImportance.olImportanceNormal;
                    break;
            }

            switch (votingoptions)
            {
                case "Accept.Reject":
                    mailitem.VotingOptions = "Accept; Reject;";
                    break;
                case "Yes.No":
                    mailitem.VotingOptions = "Yes; No;";
                    break;
                default:
                    Console.WriteLine("No voting options selected");
                    break;
            }

            if (!string.IsNullOrEmpty(this.from))
            {
                Outlook.Account account = app.Session.Accounts[this.from];

                if (account != null)
                    mailitem.SendUsingAccount = account;
            }

            mailitem.Display();
        }

        #endregion
    }
}
