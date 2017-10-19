using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleAnalyzer
{
    public class ArticleParser
    {
        private String article;

        public ArticleParser(String article)
        {
            this.article = article;
        }

        public String DefineSections()
        {
            String result = "";
            result += FindMethodsSection();
            result += FindResultsSection();
            //result = CleanResult(result);

            return result;
        }

        private string CleanResult(string result)
        {
            int start, end, count;
            while (result.Contains("<"))
            {
                start = result.IndexOf("<");
                end = result.IndexOf(">");
                count = end - start;
                if (count < 0) result = result.Remove(end, 1);

                else result = result.Remove(start, count);
            }

            return result;
        }

        private String FindMethodsSection()
        {
            String temp = "";
            String result = "";

            if (article.Contains("Methods"))
            {
                List<int> indexesOfMethods = article.AllIndexesOf("head no_bottom_margin");

                for (var i = 0; i < indexesOfMethods.Count() - 1; i++)
                {
                    int length = indexesOfMethods.ElementAt(i + 1) - indexesOfMethods.ElementAt(i);
                    temp = article.Substring(indexesOfMethods.ElementAt(i), length);
                    if (temp.Contains("Methods"))
                    {
                         result += temp + "\t\n ";

                    }
                }
            }

            return result;
        }

        private String FindResultsSection()
        {
            String temp = "";
            String result = "";

            if (article.Contains("Results"))
            {
                List<int> indexesOfMethods = article.AllIndexesOf("head no_bottom_margin");

                for (var i = 0; i < indexesOfMethods.Count() - 1; i++)
                {
                    int length = indexesOfMethods.ElementAt(i + 1) - indexesOfMethods.ElementAt(i);
                    temp = article.Substring(indexesOfMethods.ElementAt(i), length);
                    if (temp.Contains("Results"))
                    {
                        result += temp + "\t\n ";

                    }
                }
            }

            return result;
        }
    }
}
