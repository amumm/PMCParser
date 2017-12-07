using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PMCParserSetup.DataRetrieval
{
    public class IdManager
    {
        public static void GetPmcIds(String queriesFilePath, String idOutputFile, String eSearchPath, String bashPath)
        {
            Console.Write("Downloading IDs: ");


            List<String> queries = GetQueries(queriesFilePath);

            foreach(var query in queries)
            {
                String command = eSearchPath + " " + idOutputFile + " " + query;

                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = bashPath;
                myProcess.StartInfo.Arguments = command;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.Start();

                myProcess.WaitForExit();
            }

            Console.WriteLine("Finished");

        }

        private static List<String> GetQueries(String queriesFilePath)
        {
            List<String> queries = new List<string>();
            FileInfo file = new FileInfo(queriesFilePath);
            StreamReader reader = file.OpenText();
            String query;


            while((query = reader.ReadLine()) != null)
            {
                queries.Add(query);
            }
            return queries;
        }
    }
}
