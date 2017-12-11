using System;
using System.IO;
using System.Collections;
using Processing.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Processing.DataAnalysis
{
    class Analyzer
    {
        public static List<JournalPaper> Control(Configuration config, DBConnection dbc)
        {
            List<JournalPaper> result = AnalyzeAllArticles(config, dbc);
            return result;
        }

        private static List<JournalPaper> AnalyzeAllArticles(Configuration config, DBConnection dbc)
        {
            
            List<JournalPaper> papers = new List<JournalPaper>();

            var articleDirectory = new DirectoryInfo(config.ArticleDirectory);
            var files = articleDirectory.GetFiles();

            string query = @"SELECT PMC_Id FROM Article_Status
                              WHERE To_Analyze = '1'";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var idReader = cmd.ExecuteReader();

            List<String> toAnalyze = new List<String>();

            while (idReader.Read())
            {
                toAnalyze.Add(idReader.GetString(0));
            }

            idReader.Close();

            int i = 1;

            foreach (var file in files)
            {
                if (!toAnalyze.Contains(file.Name)) continue;

                StreamReader reader = file.OpenText();
                String stringFile = reader.ReadToEnd();
                reader.Close();

                JournalPaper paper = new JournalPaper();
                papers.Add(paper);
                paper.PMCID = file.Name;

                ArticleParser parser = new ArticleParser(file, stringFile, dbc, paper);

                if (parser.PassesAllFilters(i))
                {
                    String methodsAndResults = parser.DefineSections(i);
                    if (methodsAndResults != null && methodsAndResults != "" && methodsAndResults != " ")
                    {
                        parser.FindKeyWords(file.Name, methodsAndResults, i, dbc.Connection);
                        parser.FindHeaderInfo();

                        MySqlCommand updateCommand = dbc.Connection.CreateCommand();

                        updateCommand = dbc.Connection.CreateCommand();
                        updateCommand.CommandText =
                        @"UPDATE Article_Status s
                        SET Valid = 1, To_Analyze = '0'
                        WHERE s.PMC_Id = '" + file.Name + "'";
                        updateCommand.ExecuteNonQuery();
                        
                    }
                }
                i++;
            }

            Console.WriteLine("\n" + (i - 1));

            return papers;
        }
    }
}
