﻿using EmailMemoryClass.Configuration;
using EmailMemoryClass.Services;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Exception = System.Exception;
using Outlook = Microsoft.Office.Interop.Outlook;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;

namespace EmailMemoryClass 
{
    public class OutlookSearch : IDisposable
    {
        #region fields
        OutlookApp _olApp;
        public readonly TimerPlus _searchTimer;
        public readonly TimerPlus _outlookTimer;
        public bool _isSearchRunning = false;
        public bool _isSearchPaused = false;
        private bool _firstRun = true;
        private bool _hasError;
        EmailEqualityComparer _emailCompare = new EmailEqualityComparer();
        #endregion

        #region properties
        public string EmailAddress { get; set; }
        public string SearchPhrase { get; set; }
        public bool IsOutlookOpen { get; set; }
        public int SearchSize { get; set; } = 250;
        public int SearchTime { get; set; } = 0;
        public int MaxDisplayItems { get; set; } = 10;
        public int TimerInterval { get; set; } = 5;
        public double percentComplete { get; private set; } = 0;
        public List<Email> EmailsFound { get; private set; } = new List<Email>();
        public List<Email> PreviousListFound { get; private set; } = new List<Email>();

        public bool HasFirstRunComplete
        {
            get { return _firstRun; }
            set { _firstRun = value; }
        }

        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; }
        }

        public bool HasPaused
        {
            get { return _isSearchPaused; }
            set { _isSearchPaused = value; }
        }

        public bool IsRunning
        {
            get { return _isSearchRunning; }
            set { _isSearchRunning = value; }
        }
        #endregion

        #region constructors
        public OutlookSearch(AccountConfig config)
        {
            EmailAddress = config.EmailAddress;
            SearchPhrase = config.SearchTag;
            SearchSize = config.SearchSize;
            SearchTime = config.SearchTime;
            TimerInterval = config.TimerInterval;
            MaxDisplayItems = config.DisplayItems;

            _searchTimer = new TimerPlus(TimerInterval * 1000) { AutoReset = true };
            _outlookTimer = new TimerPlus(2000) { AutoReset = true };

            _searchTimer.Start();
            _outlookTimer.Start();

            // register events

            _searchTimer.Elapsed += TimerElapsed;
            _outlookTimer.Elapsed += CheckOutlookStatus;

            OnSearchErrorOccurred += ErrorHandler;
            OnFindErrorOccurred += SearchError;
            OnFindComplete += SearchComplete;
            OnServiceStart += OnStart;

            // raise on start events
            OnServiceStart?.Invoke(this, EventArgs.Empty);
        }

        async void CheckOutlookStatus(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() => {
                IsOutlookOpen = System.Diagnostics.Process.GetProcessesByName("OUTLOOK").Any();

                if (!IsOutlookOpen)
                    Logger.Log("Waiting for outlook to start", "Warning");
            });
        }

        public OutlookSearch()
        {

        }

        public OutlookSearch(bool testing)
        {
            // use testing here
        }
        #endregion

        #region eventhandlers
        public EventHandler OnServiceStart;
        public EventHandler OnSearchComplete;
        public EventHandler OnSearchErrorOccurred;
        public EventHandler OnFindComplete;
        public EventHandler OnFindErrorOccurred;

        #endregion

        #region events
        async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //SetTimer();
            //await Task.Run(RunSearch);
        }

        void SetTimer()
        {
            if (_hasError)
                _searchTimer.SetInterval(5000);
            else
                _searchTimer.SetInterval(TimerInterval * 1000);
        }

        async void OnStart(object sender, EventArgs e)
        {
            //Logger.Log("Calling OnOutlookStartup event");
            //await Task.Run(RunSearch);
        }

        void ErrorHandler(object sender, EventArgs e)
        {
            OutlookErrorOccurred(sender);
        }

        public void OutlookErrorOccurred(object sender)
        {
            HasError = true;
            _searchTimer.Interval = 5000;
        }

        void SearchComplete(object sender, EventArgs e)
        {
            var item = sender as Outlook.MailItem;
            item.Display();
        }

        private void SearchError(object sender, EventArgs e)
        {
            var exception = sender as Exception;
            MessageBox.Show(exception.Message);
        }

        #endregion

        #region methods

        public void RunSearch()
        {
            if(!_isSearchRunning)
            {
                _isSearchRunning = true;
                SearchInBox();
            }
                
        }


        /// <summary>
        /// used to asynchronously add files to EmailsFoundList
        /// </summary>
        void SearchInBox()
        {
            var watch = new Stopwatch();
            watch.Start();

            Logger.Log("Starting search for Email Account: " + EmailAddress);

            // Duplicate list for comparing equity
            PreviousListFound = EmailsFound;
            EmailsFound.Clear();
           
            // create item for later disposing of com objects
            Outlook.MAPIFolder sentBox = null;
            Outlook.Items olItems = null;
            Outlook.MailItem mailItem = null;
            Outlook.MAPIFolder account = null;

            try
            {
                _olApp = new OutlookApp();
                account = GetAccountExplicit();

                if (account == null)
                    throw new ApplicationException("Failed to extract email account");

                sentBox = account.Store.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);
                olItems = sentBox.Items;

                // order items in Email DB by date sent
                olItems.Sort("SentOn", true);

                double Counter = 0;
                _hasError = false;

                foreach (var item in olItems)
                {
                    mailItem = item as Outlook.MailItem;

                    HasError = false;

                    if (mailItem != null)
                    {
                        string body = mailItem.Body;

                        if (!string.IsNullOrEmpty(body) && body.Contains(this.SearchPhrase))
                        {
                            AddToList(new Email(mailItem));
                            
                        }
                    }

                    Counter++;
                    percentComplete = Math.Round(Counter/SearchSize * 100);

                    if (Counter >= SearchSize)
                        break;

                    if (EmailsFound.Count >= MaxDisplayItems)
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Error when running email search for account: {EmailAddress}\n Error: {ex.Message}", "Error");
                OnSearchErrorOccurred?.Invoke(ex, EventArgs.Empty);
                _firstRun = false;
            }
            finally
            {
                if (sentBox != null) Marshal.ReleaseComObject(sentBox);
                if (olItems != null) Marshal.ReleaseComObject(olItems);
                if (mailItem != null) Marshal.ReleaseComObject(mailItem);
                if (account != null) Marshal.ReleaseComObject(account);
                if (_olApp != null) Marshal.ReleaseComObject(_olApp);

                _isSearchRunning = false;  
                _firstRun = false;

                OnSearchComplete?.Invoke(this, EventArgs.Empty);
                Logger.Log("New Emails Detected, raising OnSearchComplete event");
            }

            watch.Stop();
            Logger.Log("Search Took " + watch.ElapsedMilliseconds);
            Logger.Log("Email Account Search: " + this.EmailAddress + " Emails Found: " + EmailsFound.Count);
        }

        public void FindEmail(Email email)
        {
            if(email != null)
            {
                var watch = new Stopwatch();
                watch.Start();

                Logger.Log("Searching for outlook mail item" + email.Subject);

                // create item for later disposing of com objects
                Outlook.MAPIFolder sentBox = null;
                Outlook.Items olItems = null;
                Outlook.MailItem mailItem = null;
                Outlook.MAPIFolder account = null;

                try
                {
                    _olApp = new OutlookApp();
                    account = GetAccountExplicit();

                    if (account == null)
                        throw new ApplicationException("Failed to extract email account");

                    sentBox = account.Store.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);
                    olItems = sentBox.Items;

                    // order items in Email DB by date sent
                    olItems.Sort("SentOn", true);

                    double Counter = 0;
                    HasError = false;

                    foreach (var item in olItems)
                    {
                        mailItem = item as Outlook.MailItem;

                        if (mailItem != null)
                        {
                            if (mailItem.ConversationID == email.ConversationId)
                            {
                                if (mailItem.ConversationIndex == email.ConversationIndex)
                                {
                                    Logger.Log("Email Found. Subject: " + mailItem.Subject);
                                    OnFindComplete?.Invoke(mailItem, EventArgs.Empty);
                                    break;
                                }
                            }
                        }

                        Counter++;

                        if (Counter >= SearchSize)
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Log($"Error when running email search for account: {EmailAddress}\n Error: {ex.Message}", "Error");
                    OnFindErrorOccurred?.Invoke(ex, EventArgs.Empty);
                    OnSearchErrorOccurred?.Invoke(ex, EventArgs.Empty);
                }
                finally
                {
                    if (sentBox != null) Marshal.ReleaseComObject(sentBox);
                    if (olItems != null) Marshal.ReleaseComObject(olItems);
                    if (mailItem != null) Marshal.ReleaseComObject(mailItem);
                    if (sentBox != null) Marshal.ReleaseComObject(sentBox);
                    if (_olApp != null) Marshal.ReleaseComObject(_olApp);
                }

                watch.Stop();
                Logger.Log("Mail Item Search Took " + watch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Loops through inboxes and checks for entries matching EmailAddress property
        /// </summary>
        /// <returns>returns MAPIFolder object for selected inbox</returns>
        Outlook.MAPIFolder GetAccountExplicit(string email =  null)
        {
            if (email == null)
                email = this.EmailAddress;

            Outlook.NameSpace ns = null;
            Outlook.Folders mailBoxes = null;

            try
            {
                ns = this._olApp.GetNamespace("MAPI");
                mailBoxes = ns.Folders;

                foreach (Outlook.MAPIFolder f in mailBoxes)
                {
                    if (f.Name == email)
                        return f;
                }

                throw new NullReferenceException("Email Account Not Found");
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Failed to return Outlook accounts\n Error: {ex.Message}", "Error");
                OnSearchErrorOccurred?.Invoke(ex, EventArgs.Empty);
                return null;
            }
            finally
            {
                if (ns != null) Marshal.ReleaseComObject(ns);
                if (mailBoxes != null) Marshal.ReleaseComObject(mailBoxes);
            }
        }

        void AddToList(Email email)
        {
            try
            {
                if (email == null)
                    return;

                if (!EmailsFound.Contains(email, _emailCompare))
                    EmailsFound.Add(email);
            }
            catch (System.Exception ex)
            {
                OnSearchErrorOccurred?.Invoke(ex, EventArgs.Empty);
                Logger.Log($"Failed to return Outlook accounts\n Error: {ex.Message}", "Error");
            }
        }

        bool Compare()
        {
            return EmailsFound.SequenceEqual(PreviousListFound, _emailCompare);
        }

        public void Dispose()
        {
            ExitOutlook();
        }

        void ExitOutlook()
        {
            try
            {
                //
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Failed to return Outlook accounts\n Error: {ex}", "Error");
            }
        }

        #endregion

        #region staticmethods
        static Outlook.MAPIFolder GetAccount(string email)
        {
            Outlook.NameSpace ns = null;
            Outlook.Folders mailBoxes = null;
            OutlookApp outlookApp = null;

            try
            {
                outlookApp = new OutlookApp();
                ns = outlookApp.GetNamespace("MAPI");
                mailBoxes = ns.Folders;

                foreach (Outlook.MAPIFolder f in mailBoxes)
                {
                    if (f.Name == email)
                        return f;
                }

                throw new ApplicationException("Email Account Not Found");
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Failed to return Outlook accounts\n Error: {ex}");
                return null;
            }
            finally
            {
                if (ns != null) Marshal.ReleaseComObject(ns);
                if (mailBoxes != null) Marshal.ReleaseComObject(mailBoxes);
                if (outlookApp != null) Marshal.ReleaseComObject(outlookApp);
            }
        }

        public static bool IsAccountValid(string accountName)
        {
            bool success = false;
            var stopwatch = Stopwatch.StartNew();

            Outlook.MAPIFolder mailBox = null;

            try
            {
                mailBox = GetAccount(accountName);

                if (mailBox != null)
                    success = true;
            }
            catch (System.Exception ex)
            {
                Logger.Log("Failed to verify account: " + accountName);
            }
            finally
            {
                if (mailBox != null)
                    Marshal.ReleaseComObject(mailBox);
            }

            stopwatch.Stop();
            Logger.Log("Account check took " + stopwatch.ElapsedMilliseconds + "ms");
            return success;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                if (new EmailAddressAttribute().IsValid(email))
                {
                    return true;
                }
            }
            catch (FormatException)
            {
                return false;
            }

            catch (System.Exception)
            {
                return false;
            }

            return false;
        }
        #endregion
    }

    public class EmailEqualityComparer : IEqualityComparer<Email>
    {
        // Interface for comparing two FileInfoLists
        public EmailEqualityComparer()
        {
            //ctor for equity comparer
        }

        /// <summary>
        /// compare name
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(Email x, Email y)
        {
            bool idEquity = string.Equals(x.ConversationId, x.ConversationId);
            bool conversationIndex = string.Equals(x.ConversationIndex, y.ConversationIndex);
            return idEquity && conversationIndex;
        }

        /// <summary>
        /// compare hash
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(Email e)
        {
            string s = $"{e.ConversationId}{e.ConversationIndex}";
            return s.GetHashCode();
        }
    }

}
