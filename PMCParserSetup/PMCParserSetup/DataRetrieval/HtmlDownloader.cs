using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using PMCParserSetup.Data;
using System;
using System.Collections;
using System.IO;

namespace PMCParserSetup.DataRetrieval
{
    public static class HtmlDownloader
    {
        public static void DownloadArticleByPMCId(DBConnection dbc, String htmlFileSavePath, String urlPrefix)
        {
            Console.WriteLine("Articles Downloading...");

            ArrayList idsOfArticlesToDownload = new ArrayList();

            var query = @"SELECT PMC_Id FROM Article_Status
                        WHERE Downloaded = '0' AND To_Analyze = '1'";
            var cmd = new MySqlCommand(query, dbc.Connection);

            var DbReader = cmd.ExecuteReader();

            while (DbReader.Read())
            {
                idsOfArticlesToDownload.Add(DbReader.GetString(0));
            }
            DbReader.Close();

            MySqlCommand updateCommand;

            foreach (var id in idsOfArticlesToDownload)
            {
                FileInfo htmlFile = new FileInfo(htmlFileSavePath + id);
                if (!htmlFile.Exists)
                {
                    var url = urlPrefix + id;
                    var web = new HtmlWeb();
                    var doc = web.Load(url);
                    doc.Save(htmlFileSavePath + @"\" + id);

                    updateCommand = dbc.Connection.CreateCommand();
                    updateCommand.CommandText =
                    @"UPDATE Article_Status s
                    SET Downloaded = 1
                    WHERE s.PMC_Id = '" + id + "'";
                    updateCommand.ExecuteNonQuery();

                    Console.WriteLine(id);

                }
            }

            Console.WriteLine("Articles Download: Finished");

        }

        public static void UpdateDbWithStoredArticles(DBConnection dbc, String htmlFileSavePath)
        {
            Console.Write("Updating DB with stored articles: ");


            DirectoryInfo dir = new DirectoryInfo(htmlFileSavePath);

            FileInfo[] files = dir.GetFiles();

            MySqlCommand insertCommand;

            foreach (var file in files)
            {
                insertCommand = dbc.Connection.CreateCommand();

                insertCommand.CommandText =
                @"INSERT INTO Article_Status (PMC_Id, Valid, Downloaded, To_Analyze) 
                    VALUES ('" + file.Name + @"', '3', '1', '1')
                    ON DUPLICATE KEY UPDATE 
                    Downloaded = '1'";
                insertCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Finished");
        }

        public static void Add_IDs_To_Articles_Status(String filePath, DBConnection dbc)
        {

            Console.Write("Adding IDs To Article_Status: ");


            FileInfo file = new FileInfo(filePath);
            StreamReader reader = file.OpenText();
            String stringFile = reader.ReadToEnd();
            reader.Close();

            StringReader stringReader = new StringReader(stringFile);
            String line = stringReader.ReadLine(); //Throw Out first ID in case it is bad.
            line = stringReader.ReadLine();

            MySqlCommand insertCommand;

            while (line != null)
            {
                insertCommand = dbc.Connection.CreateCommand();

                insertCommand.CommandText =
                @"INSERT INTO Article_Status (PMC_Id, Valid, Downloaded, To_Analyze) 
                    VALUES ('PMC" + line + @"', '3', '0', '1')
                    ON DUPLICATE KEY UPDATE 
                    To_Analyze = IF(Valid > 0, '1', '0')";
                insertCommand.ExecuteNonQuery();

                line = stringReader.ReadLine();
            }

            File.WriteAllText(file.FullName, String.Empty);

            Console.WriteLine("Finished");

        }
    }
}
