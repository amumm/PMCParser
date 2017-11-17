using System;
using System.IO;
using System.Collections;
using ArticleAnalyzer.Data;

namespace ArticleAnalyzer.DataAnalysis
{
    class Analyzer
    {
        public static void Control(Configuration config, DBConnection dbc)
        {
            AnalyzeAllArticles(config, dbc);
        }

        private static void AnalyzeAllArticles(Configuration config, DBConnection dbc)
        {
            String stringFile;
            StreamReader reader;
            ArticleParser parser;

            ArrayList papers = new ArrayList();

            var articleDirectory = new DirectoryInfo(config.ArticleDirectory);
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
    }
}
