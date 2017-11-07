﻿using Data;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.IO;

namespace ArticleAnalyzer.dataMining
{
    public static class HtmlDownloader
    {
        public static void AddPMCIdToDb(DBConnection dbc, String htmlFileSavePath, String urlPrefix)
        {
            ArrayList idsOfStoredArticles = new ArrayList();
            ArrayList idsOfArticlesToDownload = new ArrayList();

            string query = "SELECT * FROM Stored_Articles";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var DbReader = cmd.ExecuteReader();

            while (DbReader.Read())
            {
                idsOfStoredArticles.Add(DbReader.GetString(0));
            }
            DbReader.Close();

            query = "SELECT * FROM Articles_To_Download";
            cmd = new MySqlCommand(query, dbc.Connection);
            DbReader = cmd.ExecuteReader();

            while (DbReader.Read())
            {
                idsOfArticlesToDownload.Add(DbReader.GetString(0));
            }
            DbReader.Close();

            MySqlCommand insertCommand = dbc.Connection.CreateCommand();

            foreach(var id in idsOfArticlesToDownload)
            {
                if (idsOfStoredArticles.Contains(id)) continue;

                FileInfo htmlFile = new FileInfo(htmlFileSavePath + id);
                if(!htmlFile.Exists)
                {
                    var url = urlPrefix + id;
                    var web = new HtmlWeb();
                    var doc = web.Load(url);
                    doc.Save(htmlFileSavePath + id);

                    insertCommand = dbc.Connection.CreateCommand();
                    insertCommand.CommandText =
                    @"INSERT INTO Stored_Articles(PMC_Id) 
                    SELECT ?id FROM dual
                        WHERE NOT EXISTS(SELECT 1  FROM Stored_Articles
                            WHERE PMC_Id = ?id)";
                    insertCommand.Parameters.AddWithValue("?id", id);
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
                @"INSERT INTO Stored_Articles(PMC_Id) 
                            SELECT ?id FROM dual
                                WHERE NOT EXISTS(SELECT 1  FROM Stored_Articles
                                    WHERE PMC_Id = ?id)";
                cmd.Parameters.AddWithValue("?id", file.Name);
                cmd.ExecuteNonQuery();
            }
        }

        public static void AddArticlesToDownloadToDb(String filePath, DBConnection dbc)
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
                @"INSERT INTO Articles_To_Download(PMC_Id) 
                SELECT ?id FROM dual
                    WHERE NOT EXISTS(SELECT 1  FROM Articles_To_Download
                        WHERE PMC_Id = ?id)";
                insertCommand.Parameters.AddWithValue("?id", line);
                insertCommand.ExecuteNonQuery();

                line = stringReader.ReadLine();
            }
        }
    }

}
