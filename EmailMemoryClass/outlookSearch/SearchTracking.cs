using EmailMemoryClass.Configuration;
using EmailMemoryClass.Services;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using Outlook = Microsoft.Office.Interop.Outlook;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;

namespace EmailMemoryClass.outlookSearch
{
    public class SearchTracking
    {
        OutlookApp _olApp;
        TimerPlus _searchTimer;
        EmailEqualityComparer comparision = new EmailEqualityComparer();

        public string[] accounts = new string[] 
        {
                "david.ashworth@agilent.com",
                "customercare_uk@agilent.com",
                "customercare_ireland@agilent.com"
        };

        public string EmailAddress { get; set; }
        public string SearchPhrase { get; set; } = "00848479";
        public bool IsRunning { get; set; } = false;
        public double MaxAge { get; set; } = 21;
        public double SearchInterval { get; set; } = 3;

        public SearchTracking()
        {
            Logger.Log("Starting search tracker");
            OnServiceStart += ServiceStartup;
            _searchTimer = new TimerPlus(10 * 1000) { AutoReset = true };
            _searchTimer.Elapsed += TrackTimerElapsed;
            _searchTimer.Start();
            OnServiceStart?.Invoke(this, EventArgs.Empty);
        }

        public async void ServiceStartup(object sender, EventArgs e)
        {
            Logger.Log("Service Startup");
            await Start();
        }

        public EventHandler OnIntervalSearchComplete;
        public EventHandler OnFullSearchComplete;
        public EventHandler OnServiceStart;

        public async void TrackTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //if (!IsRunning)
            //    await RunSearch();
        }

        public async Task RunSearch(bool firstInterval, int runningTotal)
        {
            var watch = new Stopwatch();
            watch.Start();
            Logger.Log("Running Search");
            IsRunning = true;

            List<Task<SearchResultContainer>> resultList = new List<Task<SearchResultContainer>>();

            foreach (var account in accounts)
            {
                resultList.Add(Task.Run(() => SearchAllAccounts(account, firstInterval, runningTotal)));
            }

            var unsortedResults = await Task.WhenAll(resultList);
            var results = GetUniqueList(unsortedResults);

            OnIntervalSearchComplete?.Invoke(results, EventArgs.Empty);
            IsRunning = false;

            watch.Stop();
            Logger.Log($"RunSearch took {watch.ElapsedMilliseconds}ms");
        }

        public async Task Start()
        {
            double timesToRun = Math.Round(MaxAge / SearchInterval);
            Logger.Log("Calling on start method");
            bool firstSearch = true;
            int runningTotal = 0;

            for (int i = 0; i < timesToRun; i++)
            {
                if(i != 0)
                {
                    firstSearch = false;
                }

                runningTotal += (int)SearchInterval;
                await RunSearch(firstSearch, runningTotal);
            }
        }

        public List<SearchResult> GetUniqueList(SearchResultContainer[] unsortedList)
        {
            int inputLength = 0;

            List<SearchResult> sortedList = new List<SearchResult>();

            foreach (var list in unsortedList)
            {
                foreach (var item in list.Results)
                {
                    if(!sortedList.Contains(item))
                    {
                        sortedList.Add(item);
                    }
                }

                inputLength += list.Results.Count;
            }

            Logger.Log($"GetUniqueList input count {inputLength}");
            Logger.Log($"GetUniqueList output count {sortedList.Count}");
            return sortedList.OrderByDescending(x => x.Time).ToList();
        }

        public void FullSearchComplete(object sender, EventArgs e)
        {
            var itemsFound = sender as List<SearchResult>;
            Logger.Log($"Number of emails found: " + itemsFound.Count);
        }

        public SearchResultContainer SearchAllAccounts(string email, bool firstInterval, int runningTotal)
        {
            var watch = new Stopwatch();
            watch.Start();

            List<SearchResult> itemsFound = new List<SearchResult>();
            List<string> IDsFound = new List<string>();

            // create item for later disposing of com objects
            Outlook.MAPIFolder sentBox = null;
            Outlook.MailItem mailItem = null;
            Outlook.MAPIFolder account = null;
            Outlook.Items items = null;

            try
            {
                _olApp = new OutlookApp();
                account = GetAccountExplicit(email);

                if (account == null)
                    throw new ApplicationException($"Failed to extract email account: {email}");

                // get items and filter by time descending
                sentBox = account.Store.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);

                // restrict number of search items to age
                if (firstInterval)
                {
                    var lowerDT = DateTime.Now.Subtract(new TimeSpan(runningTotal, 0, 0, 0)).ToString("MM/dd/yyyy HH:mm");
                    items = sentBox.Items.Restrict($"[ReceivedTime] > '{lowerDT}'");
                    Logger.Log($"Lower Date: {lowerDT}, firstRun");
                }
                else
                {
                    var upperDT = DateTime.Now.Subtract(new TimeSpan(runningTotal + (int)SearchInterval, 0, 0, 0)).ToString("MM/dd/yyyy HH:mm");
                    var lowerDT = DateTime.Now.Subtract(new TimeSpan(runningTotal, 0, 0, 0)).ToString("MM/dd/yyyy HH:mm");
                    Logger.Log($"Lower Date: {lowerDT}, Upper Date: {upperDT}");
                    items = sentBox.Items.Restrict($"[ReceivedTime] > '{upperDT}' and [ReceivedTime] < '{lowerDT}'");
                }

                Logger.Log($"Filtered item in sentbox: {items.Count} -- Email: {email} -- Search Tag: {Guid.NewGuid()}");

                foreach (var item in items)
                {
                    mailItem = item as Outlook.MailItem;

                    if (mailItem != null)
                    {
                        string body = mailItem.Body;

                        if (body.Contains(this.SearchPhrase))
                        {
                            var mailObj = new SearchResult(mailItem);

                            if (mailObj.HasSRNumber == 1)
                            {
                                if (!IDsFound.Contains(mailObj.SRNumber))
                                {
                                    IDsFound.Add(mailObj.SRNumber);
                                    itemsFound.Add(mailObj);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Error when running email search for account: {EmailAddress}\n Error: {ex.Message}, {ex.InnerException}", "Error");
            }
            finally
            {
                if (sentBox != null) Marshal.ReleaseComObject(sentBox);
                if (items != null) Marshal.ReleaseComObject(items);
                if (mailItem != null) Marshal.ReleaseComObject(mailItem);
                if (account != null) Marshal.ReleaseComObject(account);
                if (_olApp != null) Marshal.ReleaseComObject(_olApp);
                watch.Stop();
                Logger.Log($"Search complete: {email}. Items Found: {itemsFound.Count}. Search Took: {watch.ElapsedMilliseconds/1000}s");
            }

            return new SearchResultContainer(itemsFound);
        }

        /// <summary>
        /// Loops through inboxes and checks for entries matching EmailAddress property
        /// </summary>
        /// <returns>returns MAPIFolder object for selected inbox</returns>
        Outlook.MAPIFolder GetAccountExplicit(string email = null)
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
                return null;
            }
            finally
            {
                if (ns != null) Marshal.ReleaseComObject(ns);
                if (mailBoxes != null) Marshal.ReleaseComObject(mailBoxes);
            }
        }

        public void FindLastInConversation(Outlook.MailItem mailItem)
        {
            if (mailItem is Outlook.MailItem)
            {
                // Determine the store of the mail item. 
                Outlook.Folder folder = mailItem.Parent as Outlook.Folder;
                Outlook.Store store = folder.Store;
                if (store.IsConversationEnabled)
                {
                    // Obtain a Conversation object. 
                    Outlook.Conversation conv = mailItem.GetConversation();

                    // Check for null Conversation. 
                    if (conv != null)
                    {
                        // Obtain Table that contains rows                      
                        Outlook.Table table = conv.GetTable();
                        int count = table.GetRowCount();
                        Logger.Log("Conversation Items Count: " + count.ToString());

                        table.MoveToStart();

                        if (!table.EndOfTable)
                        {
                            // lastRow conatins the last item from the conversation
                            Outlook.Row lastRow = table.GetNextRow();

                            //Logger.Log(lastRow["Subject"] + " Modified: " + lastRow["LastModificationTime"]);
                        }
                    }
                }
            }
        }
    }

    public class EmailEqualityComparer : IEqualityComparer<SearchResult>
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
        public bool Equals(SearchResult x, SearchResult y)
        {
            bool idEquity = string.Equals(x.SRNumber, x.SRNumber);
            bool validContainer = x.HasSRNumber == 1 && y.HasSRNumber == 1;
            return idEquity && validContainer;
        }

        /// <summary>
        /// compare hash
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(SearchResult e)
        {
            string s = $"{e.SRNumber}{e.ConversationIndex}";
            return s.GetHashCode();
        }
    }
}
