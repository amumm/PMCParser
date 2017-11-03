using MySql.Data.MySqlClient;
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

        public void FindHeaderInfo(MySqlConnection connection)
        {
            List<String> attributes = new List<string>();
            attributes.Add("journal_title");
            attributes.Add("title");
            attributes.Add("authors");
            attributes.Add("date");
            attributes.Add("issue");
            attributes.Add("volume");

            String prefix = "citation_";
            String tag = "";
            int tagLength = 0;

            foreach (String attribute in attributes)
            {
                prefix = "citation_";
                tag = prefix + attribute;
                tagLength = tag.Length;

                FindContent(tag, tagLength);
            }
            Console.WriteLine("");

            //MySqlCommand cmd = connection.CreateCommand();
            //cmd.CommandText = "INSERT INTO keywords(keyword, fileName) VALUES(?keyword, ?fileName)";
            //cmd.Parameters.AddWithValue("?keyword", word);
            //cmd.Parameters.AddWithValue("?fileName", fileName);
            //cmd.ExecuteNonQuery();
        }

        private void FindContent(String tag, int tagLength)
        {
            int start, end;
            String content = "";
            if (article.Contains(tag))
            {
                start = article.IndexOf(tag);
                content = article.Substring(start + tagLength + 11); //move foraward to the atart of authors
                end = content.IndexOf("/");
                content = content.Substring(0, end - 2);
                Console.WriteLine(content);
            }
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
