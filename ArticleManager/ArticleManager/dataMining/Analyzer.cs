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
            //FileInfo file = new FileInfo("C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//Wget attempt//html_download//PMC_joe");
            //String stringFile;
            //StreamReader reader;
            //ArticleParser parser;

            //reader = file.OpenText();
            //stringFile = reader.ReadToEnd();
            //parser = new ArticleParser(stringFile);
            //String result = parser.DefineSections();
            //if (result != null && result != "" && result != " ")
            //{
            //    Console.WriteLine(file.FullName + result);

            //}
            //Manager manage = new Manager("C://Users//mumm9//Documents//ISU//Fall2017//COMS 490//pdfArticles//oa_pdf");
            //manage.renameAndRelocate();

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
                        Console.WriteLine(i + ") " + file.FullName + result);

                    }
                    else
                        Console.WriteLine(i + ") No Methods or Results");
                }
                else
                {
                    Console.WriteLine(i + ") Failed Filter");
                }
                i++;

                reader.Close();
            }

            Console.WriteLine(i);

        }

    }
}
