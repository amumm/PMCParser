using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArticleAnalyzer.DataRetrieval
{
    public class IdManager
    {
        public static void GetPmcIds(List<String> queries, String idOutputFile, String eSearchPath, String bashPath)
        {

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
                Console.WriteLine("finished");
                

            }
        }
    }
}
