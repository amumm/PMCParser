using System;
using System.IO;
using System.Text;
using Data;
using MySql.Data.MySqlClient;
using System.Collections;
using ArticleAnalyzer.dataMining;
using ArticleAnalyzer.Data;
using System.Diagnostics;

namespace ArticleAnalyzer
{
    class Analyzer
    {

        static void Main(string[] args)
        {
            //GetPmcIds();

            String parentSubjectDirectory = @"D:\UKBiobank\Neuroimages\result";
            String jobFilesResultDirectory = @"D:\Batch_and_Job\Jobs";
            String pathToSubjectFoldersOnServer = @"R:\FSHN\Willette_Research\UkBiobank\Neuroimages\Resting_state_FMRI\Unzipped\Subjects";
            String pathToJobFilesOnServer = @"R:\FSHN\Willette_Research\UkBiobank\Neuroimages\Resting_state_FMRI\Unzipped\Jobs";
            String batchFilesResultDirectory = @"D:\Batch_and_Job\Batch";

            Manager preProc = new Manager(parentSubjectDirectory, jobFilesResultDirectory);
            preProc.GeneratePreProcJobFiles(pathToSubjectFoldersOnServer);
            preProc.GeneratePreProcBatchFiles(pathToJobFilesOnServer, batchFilesResultDirectory);

            //DBConnection dbc = SetUpDBConnection();
            //if (dbc.IsConnected())
            //{
            //    Console.WriteLine("Conncted Successfully");
            //    //DBManagement.AddDataTypesToTable(@"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\Data_To_Upload\memory.txt", dbc);
            //    //getHtml(dbc);
            //    AnalyzeAllArticles(dbc);
            //    dbc.Close();
            //}
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
                parser = new ArticleParser(file, stringFile, dbc, paper);

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

        public static void GetPmcIds()
        {
            String command = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\Initial_ID_Collection\collect_ids C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\Initial_ID_Collection\out.txt 'opsin gene conversion'";
            String bash = @"C:\cygwin64\bin\bash.exe";
            
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = bash;
            myProcess.StartInfo.Arguments = command;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.Start();

            myProcess.WaitForExit();
            string strOutput = myProcess.StandardOutput.ReadToEnd();
            Console.WriteLine(strOutput);
        }

        public static void GetHtml(DBConnection dbc)
        {
            //HtmlDownloader.AddArticlesToDownloadToDb(
            //    @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\wget_pmc_download\pmc_id_paper_list.txt",
            //    dbc);
            //HtmlDownloader.UpdateDbWithStoredArticles(dbc, @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\html_download");
            HtmlDownloader.DownloadArticleByPMCId(dbc, 
                @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\html_download\",
                @"https://www.ncbi.nlm.nih.gov/pmc/articles/");

        }
    }
}
