using System;
using System.IO;

namespace FileManipulation.Tasks
{
    class PreProcCreator
    {

        private DirectoryInfo ParentDirectory;

        private String ResultPath;

        public PreProcCreator(String path, String resultPath)
        {
            ParentDirectory = new DirectoryInfo(path);
            ResultPath = resultPath;
        }

        public void GeneratePreProcJobFiles(String pathToSubjectFoldersOnServer, String dumbyJobFilePath)
        {

            FileInfo jobFile = new FileInfo(dumbyJobFilePath);
            StreamReader reader = jobFile.OpenText();
            String stringFile = reader.ReadToEnd();

            var openBrackets = stringFile.AllIndexesOf("{");
            var closeBrackets = stringFile.AllIndexesOf("}");

            String beginningOfFile = stringFile.Substring(0, openBrackets[2] + 1);
            String endOfFile = stringFile.Substring(closeBrackets[1], stringFile.Length - closeBrackets[1]);
            String result = beginningOfFile;

            //Console.WriteLine(beginningOfFile + endOfFile);

            DirectoryInfo[] directories = ParentDirectory.GetDirectories();

            foreach (var directory in directories)
            {
                FileInfo[] files = directory.GetFiles();
                foreach (var file in files)
                {
                    if (file.Name.Contains(".nii") && file.Name.Contains("afI")
                        && !(file.Name.Contains("rafI") || file.Name.Contains("mean"))
                        && !(file.Name.Contains("001") || file.Name.Contains("002") || file.Name.Contains("003")))
                    {
                        result += "\'" + pathToSubjectFoldersOnServer + @"\" + directory.Name + @"\" + file.Name + ",1\'\n\t\t\t\t\t\t\t\t\t\t";
                    }
                }
                result += endOfFile;
                //Console.WriteLine(result);
                System.IO.File.WriteAllText(ResultPath + @"\" + directory.Name + "_job.m", result);
                result = beginningOfFile;
            }

        }

        public void GeneratePreProcBatchFiles(String pathtoJobFilesOnServer, String pathToBatchResult, String dumbyBatchFilePath)
        {
            DirectoryInfo locationOfJobFiles = new DirectoryInfo(ResultPath);
            FileInfo batchFile = new FileInfo(dumbyBatchFilePath);
            StreamReader reader = batchFile.OpenText();
            String stringFile = reader.ReadToEnd();

            var openBrackets = stringFile.AllIndexesOf("{");
            var closeBrackets = stringFile.AllIndexesOf("}");

            String beginningOfFile = stringFile.Substring(0, openBrackets[0] + 1);
            String endOfFile = stringFile.Substring(closeBrackets[0], stringFile.Length - closeBrackets[0]);
            String result = beginningOfFile;

            //Console.WriteLine(beginningOfFile + endOfFile);

            FileInfo[] files = locationOfJobFiles.GetFiles();
            foreach (var file in files)
            {
                result += "\'" + pathtoJobFilesOnServer + @"\" + file.Name + "\',\n\t\t\t";

            }
            result += endOfFile;
            //Console.WriteLine(result);
            System.IO.File.WriteAllText(pathToBatchResult + @"\batchFile.m", result);

        }
    }
}
