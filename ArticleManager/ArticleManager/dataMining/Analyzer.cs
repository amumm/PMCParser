using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using MySql.Data.MySqlClient;

namespace ArticleAnalyzer
{
    class Analyzer
    {

        static void Main(string[] args)
        {
            //MoveFiles();
            //DirectoryInfo source = new DirectoryInfo("D://UKBiobank//Neuroimages//result");
            //DirectoryInfo result = new DirectoryInfo("D://UKBiobank//Neuroimages//nifty");

            //DirectoryInfo[] sources = source.GetDirectories();
            //FileInfo[] results = result.GetFiles();

            //String sourceName = "";
            //String resultName = "";
            //int foundCount = 0;
            //int numNotFound = 0;

            //bool isFound = false;
            //for (int i = 0; i < sources.Length; i++)
            //{
            //    isFound = false;
            //    foundCount = 0;
            //    for (int j = 0; j < results.Length; j++)
            //    {
            //        sourceName = sources[i].Name.Substring(0, 7);
            //        resultName = results[j].Name.Substring(0, 7);
            //        if (sourceName == resultName)
            //        {
            //            if (isFound)
            //            {
            //                foundCount++;
            //            }
            //            isFound = true;

            //        }
            //    }
            //    if (!isFound)
            //    {
            //        Console.WriteLine("Source: " + sourceName + " Result: " + resultName);
            //        numNotFound++;

            //    }
            //    if (foundCount > 1) Console.WriteLine(sourceName);
            //}
            //Console.WriteLine(numNotFound);

            DBConnection dbc = SetUpDBConnection();
            if (dbc.IsConnected()) Console.WriteLine("Conncted Successfully");
            //AnalyzeSingleArticle();
            AnalyzeAllArticles(dbc);
            //StringBuilder sb = new StringBuilder();

            //string query = "SELECT keyword, fileName FROM keywords";
            //var cmd = new MySqlCommand(query, dbc.Connection);
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    string keyword = reader.GetString(0);
            //    string filename = reader.GetString(1);
            //    if(keyword != null && filename != null)
            //    {
            //        var newLine = $"{keyword}, {filename}";
            //        sb.AppendLine(newLine);

            //    }
            //    //Console.WriteLine("Keyword: " + keyword + ", File: " + filename);
            //}

            //File.WriteAllText(@"C:\Users\mumm9\Documents\ISU\Fall2017\COMS 490\repos\PMCParser\test.csv", sb.ToString());

            dbc.Close();
        }

        public static void MoveFiles()
        {
            Manager manager = new Manager(
                "D://UKBiobank//Neuroimages//result",
                "D://UKBiobank//Neuroimages//nifty//");
            manager.RenameAndRelocate();
        }

        public static void AnalyzeSingleArticle()
        {
            FileInfo file = new FileInfo("C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//Wget attempt//html_download//PMC_joe");
            String stringFile;
            StreamReader reader;
            ArticleParser parser;

            reader = file.OpenText();
            stringFile = reader.ReadToEnd();
            parser = new ArticleParser(stringFile);
            String result = parser.DefineSections();
            if (result != null && result != "" && result != " ")
            {
                Console.WriteLine(file.Name + result);

            }
        }

        public static void AnalyzeAllArticles(DBConnection dbc)
        {
            String referenceKeyWordPath = "C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//Wget attempt//referenceKeyWords.txt";
            String validKeyWordPath = "C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//Wget attempt//validKeyWords.txt";

            Filter filter = new Filter(referenceKeyWordPath, validKeyWordPath);

            String stringFile;
            StreamReader reader;
            ArticleParser parser;
            var articleDirectory = new DirectoryInfo("C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//Wget attempt//html_download");
            var files = articleDirectory.GetFiles();

            int i = 1;

            foreach (var file in files)
            {
                reader = file.OpenText();
                stringFile = reader.ReadToEnd();
                parser = new ArticleParser(stringFile);

                if (filter.PassesAllFilters(stringFile))
                {
                    String result = parser.DefineSections();


                    if (result != null && result != "" && result != " ")
                    {
                        parser.FindHeaderInfo(dbc.Connection);
                        Console.WriteLine(file.Name);
                        Console.WriteLine(result);
                        Console.WriteLine("");
                        //continue;
                        if (!filter.FindKeyWords(file.Name, result, i, dbc.Connection))
                        {
                            //Console.WriteLine(i + ") Failed:\n\tReason: Neither Methods nor Results Contain Keywords\n\tFile: " + file.Name);

                        }

                    }
                    else { }
                        //Console.WriteLine(i + ") Failed:\n\tReason: No Methods or Result\n\tFile: " + file.Name);
                }
                else
                {
                    //Console.WriteLine(i + ") Failed:\n\tReason: Failed Filter\n\tFile: " + file.Name);
                }
                i++;

                reader.Close();
            }

            Console.WriteLine("\n" + (i - 1));
        }

        public static DBConnection SetUpDBConnection()
        {
            DBConnection dbc = new DBConnection("mysql.cs.iastate.edu", "db490amumm", "dbu490amumm", "zjsb8J9H4ajW");
            return dbc;
        }
    }
}
