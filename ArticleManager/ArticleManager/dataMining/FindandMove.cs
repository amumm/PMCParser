using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;

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

            foreach (var subjectDirectory in set1) //all subject folders
            {
                set2 = subjectDirectory.GetDirectories();
                newName = subjectDirectory.Name.Substring(0, 7) + "_vol_bl.nii.gz";

                foreach (var t1Directory in set2) // all folders in subject folder
                {
                    set3 = t1Directory.GetDirectories();

                    foreach (var t1FastDirectory in set3) // all folders in T1 folder
                    {
                        files = t1FastDirectory.GetFiles();

                        foreach (var file in files) // all files in T1_fast folder
                        {
                            if (file.Name.Contains("T1_brain_pve_1")){
                                file.CopyTo(ResultPath + newName);
                            }
                        }
                    }
                }
            }
        }
    }
}
