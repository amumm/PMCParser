using System;
using System.Diagnostics;

namespace ArticleAnalyzer.DataRetrieval
{
    public class IdManager
    {
        public static void GetPmcIds(String[] query, String idOutputFile, String eSearchPath, String bashPath)
        {
            String command = bashPath + " " + idOutputFile + " " + query[0];

            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = bashPath;
            myProcess.StartInfo.Arguments = command;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.Start();

            myProcess.WaitForExit();
            string strOutput = myProcess.StandardOutput.ReadToEnd();
            Console.WriteLine(strOutput);
        }
    }
}
