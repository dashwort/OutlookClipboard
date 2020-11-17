using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailMemoryClass.outlookSearch;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace EmailMemoryClass
{
    public class SqliteDataAccess
    {
        public static List<SearchResult> LoadResults()
        {
            // opens connection and using statement will close connection and stops errant connections
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<SearchResult>("select * from SearchResults", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveResult(SearchResult result)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                //cnn.Execute("insert into SearchResults (Account, To, Cc, Bcc, ConversationIndex, ConversationID, Subject, Body, TimeSent, SRNumber, HasSRNumber) " +
                //    "values (@Account, @To, @Cc, @Bcc, @ConversationIndex, @ConversationID, @Subject, @Body, @TimeSent, @SRNumber, @HasSRNumber)", result);


                cnn.Execute("insert into SearchResults (SRNumber, Account, EmailTo, Cc, Bcc, ConversationIndex, ConversationID, Subject, Body) " +
                    "values (@SRNumber, @Account, @EmailTo, @Cc, @Bcc, @ConversationIndex, @ConversationID, @Subject, @Body)", result);
            }
        }

        public static void SaveResults(List<SearchResult> results)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                //cnn.Execute("insert into SearchResults (Account, To, Cc, Bcc, ConversationIndex, ConversationID, Subject, Body, TimeSent, SRNumber, HasSRNumber) " +
                //    "values (@Account, @To, @Cc, @Bcc, @ConversationIndex, @ConversationID, @Subject, @Body, @TimeSent, @SRNumber, @HasSRNumber)", result);


                cnn.Execute("insert into SearchResults (SRNumber, Account, EmailTo, Cc, Bcc, ConversationIndex, ConversationID, Subject, Body, HasSRNumber, Time) " +
                    "values (@SRNumber, @Account, @EmailTo, @Cc, @Bcc, @ConversationIndex, @ConversationID, @Subject, @Body, @HasSRNumber, @Time)", results);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
