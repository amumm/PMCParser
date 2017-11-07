using System;
using System.IO;
using System.Text;
using Data;
using MySql.Data.MySqlClient;
using System.Collections;
using ArticleAnalyzer.dataMining;

namespace ArticleAnalyzer
{
    class Analyzer
    {

        static void Main(string[] args)
        {
            
            DBConnection dbc = SetUpDBConnection();
            if (dbc.IsConnected())
            {
                Console.WriteLine("Conncted Successfully");
                getHtml(dbc);
                //AnalyzeAllArticles(dbc);
                dbc.Close();
            }
        }

        public static void MoveFiles()
        {
            Manager manager = new Manager(
                "D://UKBiobank//Neuroimages//result",
                "D://UKBiobank//Neuroimages//nifty//");
            manager.RenameAndRelocate();
        }

        public void WriteToExcel(DBConnection dbc)
        {
            StringBuilder sb = new StringBuilder();

            string query = "SELECT keyword, fileName FROM keywords";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string keyword = reader.GetString(0);
                string filename = reader.GetString(1);
                if (keyword != null && filename != null)
                {
                    var newLine = $"{keyword}, {filename}";
                    sb.AppendLine(newLine);

                }
                //Console.WriteLine("Keyword: " + keyword + ", File: " + filename);
            }

            File.WriteAllText(@"C:\Users\mumm9\Documents\ISU\Fall2017\COMS 490\repos\PMCParser\test.csv", sb.ToString());
        }

        public static void AnalyzeAllArticles(DBConnection dbc)
        {
            String validKeyWordPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\wget_pmc_download\validKeyWords.txt";

            String stringFile;
            StreamReader reader;
            ArticleParser parser;

            ArrayList papers = new ArrayList();

            var articleDirectory = new DirectoryInfo("C://Users//mumm9//Documents//ISU//Fall2017//COMS490//repos//html_download");
            var files = articleDirectory.GetFiles();

            int i = 1;

            foreach (var file in files)
            {
                reader = file.OpenText();
                stringFile = reader.ReadToEnd();
                JournalPaper paper = new JournalPaper();
                papers.Add(paper);
                parser = new ArticleParser(file, stringFile, validKeyWordPath, dbc, paper);

                if (parser.PassesAllFilters())
                {
                    String methodsAndResults = parser.DefineSections();
                    if (methodsAndResults != null && methodsAndResults != "" && methodsAndResults != " ")
                    {
                        parser.FindHeaderInfo();
                        Console.WriteLine(file.Name);
                        //Console.WriteLine(methodsAndResults);
                        Console.WriteLine("");
                        parser.FindKeyWords(file.Name, methodsAndResults, i, dbc.Connection);
                        
                    }
                    else
                        Console.WriteLine("Failed:\n\tReason: No Methods or Result\n\tFile: " + file.Name);
                }

                reader.Close();
            }

            Console.WriteLine("\n" + (i - 1));
        }

        public static DBConnection SetUpDBConnection()
        {
            DBConnection dbc = new DBConnection("mysql.cs.iastate.edu", "db490amumm", "dbu490amumm", "zjsb8J9H4ajW");
            return dbc;
        }

        public static void getHtml(DBConnection dbc)
        {
            //HtmlDownloader.AddArticlesToDownloadToDb(
            //    @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\wget_pmc_download\pmc_id_paper_list.txt",
            //    dbc);
            //HtmlDownloader.UpdateDbWithStoredArticles(dbc, @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\html_download");
            HtmlDownloader.AddPMCIdToDb(dbc, 
                @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\html_download\",
                @"https://www.ncbi.nlm.nih.gov/pmc/articles/");

        }
    }
}
