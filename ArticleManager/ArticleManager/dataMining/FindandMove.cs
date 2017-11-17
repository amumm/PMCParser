using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ArticleAnalyzer
{
    class Manager
    {

        private DirectoryInfo ParentDirectory;

        private String ResultPath;

        public Manager(String path, String resultPath)
        {
            ParentDirectory = new DirectoryInfo(path);
            ResultPath = resultPath;
        }

        public void RenameAndRelocate()
        {
            DirectoryInfo[] set1, set2, set3;
            FileInfo[] files;
            String newName;

            set1 = ParentDirectory.GetDirectories();
            Console.WriteLine(set1.Length);
            GrantAccess(ParentDirectory.FullName);
            foreach (var subjectDirectory in set1) //all subject folders
            {
                set2 = subjectDirectory.GetDirectories();
                newName = subjectDirectory.Name.Substring(0, 7) + "_vol_bl.nii.gz";
                FileInfo newFile = new FileInfo(ResultPath + newName);

                foreach (var t1Directory in set2) // all folders in subject folder
                {
                    set3 = t1Directory.GetDirectories();

                    foreach (var t1FastDirectory in set3) // all folders in T1 folder
                    {
                        if (t1FastDirectory.Name.Contains("unusable"))
                        {
                            subjectDirectory.MoveTo("D://UKBiobank//Neuroimages//unusable//" + subjectDirectory.Name);
                            Console.WriteLine(subjectDirectory.Name);
                            break;
                        }
                        files = t1FastDirectory.GetFiles();

                        foreach (var file in files) // all files in T1_fast folder
                        {
                            if (file.Name.Contains("T1_brain_pve_1") && newFile == null)
                            {
                                file.CopyTo(ResultPath + newName);
                                Console.WriteLine(newName);
                            }
                        }

                    }
                }
            }
        }

        public void GeneratePreProcJobFiles(String pathToSubjectFoldersOnServer){

            FileInfo jobFile = new FileInfo(@"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\Parent\Batch_RestingStatefMRI_Preproc(A,R,W,S)_job.m");
            StreamReader reader = jobFile.OpenText();
            String stringFile = reader.ReadToEnd();

            var openBrackets = stringFile.AllIndexesOf("{");
            var closeBrackets = stringFile.AllIndexesOf("}");

            String beginningOfFile = stringFile.Substring(0, openBrackets[2] + 1);
            String endOfFile = stringFile.Substring(closeBrackets[1], stringFile.Length - closeBrackets[1]);
            String result = beginningOfFile;

            //Console.WriteLine(beginningOfFile + endOfFile);

            DirectoryInfo[] directories = ParentDirectory.GetDirectories();

            foreach(var directory in directories)
            {
                FileInfo[] files = directory.GetFiles();
                foreach(var file in files){
                    if (file.Name.Contains(".nii") && file.Name.Contains("afI")
                        && !(file.Name.Contains("rafI") || file.Name.Contains("mean"))
                        && !(file.Name.Contains("001") || file.Name.Contains("002") || file.Name.Contains("003")))
                    {
                        result += "\'" + pathToSubjectFoldersOnServer + @"\" +  directory.Name + @"\" + file.Name + ",1\'\n\t\t\t\t\t\t\t\t\t\t";
                    }
                }
                result += endOfFile;
                //Console.WriteLine(result);
                System.IO.File.WriteAllText(ResultPath + @"\" + directory.Name + "_job.m", result);
                result = beginningOfFile;
            }

        }

        public void GeneratePreProcBatchFiles(String pathtoJobFilesOnServer, String pathToBatchResult)
        {
            DirectoryInfo locationOfJobFiles = new DirectoryInfo(ResultPath);
            FileInfo batchFile = new FileInfo(@"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\Parent\Batch_RestingStatefMRI_Preproc(A,R,W,S).m");
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
                result += "\'" + pathtoJobFilesOnServer + @"\" + file.Name + "\'";
               
            }
            result += endOfFile;
            //Console.WriteLine(result);
            System.IO.File.WriteAllText(pathToBatchResult + @"\batchFile.m", result);

        }

        private static void GrantAccess(string file)
        {
            bool exists = System.IO.Directory.Exists(file);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(file);
                Console.WriteLine("The Folder is created Sucessfully");
            }
            else
            {
                Console.WriteLine("The Folder already exists");
            }
            DirectoryInfo dInfo = new DirectoryInfo(file);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

        }
    }
}
