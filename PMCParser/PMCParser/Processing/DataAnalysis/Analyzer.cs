using System;
using System.IO;
using System.Collections;
using Processing.Data;
using MySql.Data.MySqlClient;

namespace Processing.DataAnalysis
{
    class Analyzer
    {
        public static void Control(Configuration config, DBConnection dbc)
        {
            AnalyzeAllArticles(config, dbc);
        }

        private static void AnalyzeAllArticles(Configuration config, DBConnection dbc)
        {
            
            ArrayList papers = new ArrayList();

            var articleDirectory = new DirectoryInfo(config.ArticleDirectory);
            var files = articleDirectory.GetFiles();

            int i = 1;

            foreach (var file in files)
            {
                
                StreamReader reader = file.OpenText();
                String stringFile = reader.ReadToEnd();
                reader.Close();

                JournalPaper paper = new JournalPaper();
                papers.Add(paper);

                ArticleParser parser = new ArticleParser(file, stringFile, dbc, paper);

                if (parser.PassesAllFilters(i))
                {
                    String methodsAndResults = parser.DefineSections(i);
                    if (methodsAndResults != null && methodsAndResults != "" && methodsAndResults != " ")
                    {
                        parser.FindKeyWords(file.Name, methodsAndResults, i, dbc.Connection);
                        //parser.FindHeaderInfo();

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
        }
    }
}
