using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleAnalyzer
{
    class Analyzer
    {

        static void Main(string[] args)
        {
            //AnalyzeSingleArticle();
            AnalyzeAllArticles();
        }

        public static void MoveFiles()
        {
            Manager manage = new Manager("C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//pdfArticles//oa_pdf");
            manage.RenameAndRelocate();
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
                Console.WriteLine(file.FullName + result);

            }
        }

        public static void AnalyzeAllArticles()
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
                        if (!filter.FindKeyWords(file.FullName, result, i))
                        {
                            Console.WriteLine(i + ") Failed:\n\tReason: Neither Methods nor Results Contain Keywords\n\tFile: " + file.FullName);

                        }

                    }
                    else
                        Console.WriteLine(i + ") Failed:\n\tReason: No Methods or Result\n\tFile: " + file.FullName);
                }
                else
                {
                    Console.WriteLine(i + ") Failed:\n\tReason: Failed Filter\n\tFile: " + file.FullName);
                }
                i++;

                reader.Close();
            }

            Console.WriteLine("\n" + (i - 1));
        }
    }
}
