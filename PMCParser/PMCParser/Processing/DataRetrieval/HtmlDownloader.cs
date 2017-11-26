using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using Processing.Data;
using System;
using System.Collections;
using System.IO;

namespace Processing.DataRetrieval
{
    public static class HtmlDownloader
    {
        public static void DownloadArticleByPMCId(DBConnection dbc, String htmlFileSavePath, String urlPrefix)
        {
            ArrayList idsOfArticlesToDownload = new ArrayList();

            var query = @"SELECT PMC_Id FROM Articles_To_Download
                        WHERE PMC_Id NOT IN 
                              (SELECT(PMC_Id)
                               FROM Article_Status 
                               WHERE Downloaded = 1)";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var DbReader = cmd.ExecuteReader();

            while (DbReader.Read())
            {
                idsOfArticlesToDownload.Add(DbReader.GetString(0));
            }
            DbReader.Close();

            MySqlCommand insertCommand = dbc.Connection.CreateCommand();

            foreach(var id in idsOfArticlesToDownload)
            {
                FileInfo htmlFile = new FileInfo(htmlFileSavePath + id);
                if(!htmlFile.Exists)
                {
                    var url = urlPrefix + id;
                    var web = new HtmlWeb();
                    var doc = web.Load(url);
                    doc.Save(htmlFileSavePath + @"\"  + id);

                    insertCommand = dbc.Connection.CreateCommand();
                    insertCommand.CommandText =
                    @"INSERT INTO Stored_Articles(PMC_Id) 
                    SELECT ?id, ?status, ?valid FROM dual
                                WHERE NOT EXISTS(SELECT 1  FROM Article_Status
                                    WHERE PMC_Id = ?id)";
                    cmd.Parameters.AddWithValue("?id", id);
                    cmd.Parameters.AddWithValue("?Downloaded", 1);
                    cmd.Parameters.AddWithValue("?valid", 3);
                    insertCommand.ExecuteNonQuery();
                    Console.WriteLine(id);

                }
            }
        }

        public static void UpdateDbWithStoredArticles(DBConnection dbc, String htmlFileSavePath)
        {
            DirectoryInfo dir = new DirectoryInfo(htmlFileSavePath);

            FileInfo[] files = dir.GetFiles();

            MySqlCommand cmd = dbc.Connection.CreateCommand();
            
            foreach (var file in files)
            {
                cmd = dbc.Connection.CreateCommand();
                cmd.CommandText =
                @"INSERT INTO Article_Status(PMC_Id, Status, Valid) 
                            SELECT ?id, ?status, ?valid FROM dual
                                WHERE NOT EXISTS(SELECT 1  FROM Article_Status
                                    WHERE PMC_Id = ?id)";
                cmd.Parameters.AddWithValue("?id", file.Name);
                cmd.Parameters.AddWithValue("?Downloaded", 1);
                cmd.Parameters.AddWithValue("?valid", 3);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Add_IDs_To_Articles_To_Download(String filePath, DBConnection dbc)
        {

            MySqlCommand insertCommand;

            insertCommand = dbc.Connection.CreateCommand();
            insertCommand.CommandText =
            @"INSERT INTO Articles_To_Download(PMC_Id) 
            SELECT a.PMC_Id FROM Articles_To_Analyze a
                WHERE NOT EXISTS(SELECT d.PMC_Id  FROM Articles_To_Download d
                                WHERE a.PMC_Id = d.PMC_Id)
                                AND NOT EXISTS(SELECT s.PMC_Id FROM Article_Status s
                                WHERE a.PMC_Id = s.PMC_Id AND s.Downloaded = 1)";
            insertCommand.ExecuteNonQuery();


        }

        public static void Add_IDs_To_Articles_To_Analyze(String filePath, DBConnection dbc)
        {

            FileInfo file = new FileInfo(filePath);
            StreamReader reader = file.OpenText();
            String stringFile = reader.ReadToEnd();

            StringReader stringReader = new StringReader(stringFile);
            String line = stringReader.ReadLine();

            MySqlCommand insertCommand;

            while (line != null)
            {
                insertCommand = dbc.Connection.CreateCommand();
                insertCommand.CommandText =
                @"INSERT INTO Articles_To_Analyze(PMC_Id) 
                SELECT ?id FROM dual
                    WHERE NOT EXISTS(SELECT 1  FROM Articles_To_Analyze
                        WHERE PMC_Id = ?id)
                        AND NOT EXISTS(SELECT 1 FROM Article_Status 
                        WHERE PMC_Id = ?id AND Valid = 0)";
                insertCommand.Parameters.AddWithValue("?id", line);
                insertCommand.ExecuteNonQuery();

                line = stringReader.ReadLine();
            }
        }
    }
}
