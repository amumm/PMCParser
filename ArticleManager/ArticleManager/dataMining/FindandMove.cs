using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;

namespace ArticleAnalyzer
{
    class Manager
    {
        private DirectoryInfo parentDirectory;

        public Manager(String path)
        {
            parentDirectory = new DirectoryInfo(path);
        }

        public void RenameAndRelocate()
        {
            DirectoryInfo[] set1, set2;
            FileInfo[] files;
            String fileName, newName;
            int newNameStartPoint;


            var result = "C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//pdfArticles//result//";


            set1 = parentDirectory.GetDirectories();

            foreach (var directory in set1)
            {
                set2 = directory.GetDirectories();

                foreach (var fileDir in set2)
                {
                    files = fileDir.GetFiles();

                    foreach (var file in files)
                    {
                        fileName = file.ToString();
                        newNameStartPoint = fileName.IndexOf("PMC");
                        newName = fileName.Substring(newNameStartPoint);
                        var doesFileExist = new FileInfo(result + newName);
                        if (doesFileExist == null)
                        {
                            file.CopyTo(result + newName);
                        }
                    }
                }
            }
        }
    }
}
