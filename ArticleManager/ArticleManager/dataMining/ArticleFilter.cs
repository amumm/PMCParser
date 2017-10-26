using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.IO;

namespace ArticleAnalyzer
{
    public class Filter
    {

        public ArrayList referenceKeyWords;

        public ArrayList validKeyWords;

        public Filter(String referenceKeyWordsPath, String validKeyWordsPath)
        {
            FileInfo referenceKeyWordFile = new FileInfo(referenceKeyWordsPath);
            var referenceReader = referenceKeyWordFile.OpenText();

            FileInfo validKeyWordFile = new FileInfo(validKeyWordsPath);
            var validReader = validKeyWordFile.OpenText();

            String line;

            referenceKeyWords = new ArrayList();
            validKeyWords = new ArrayList();

            while ((line = referenceReader.ReadLine()) != null)
            {
                referenceKeyWords.Add(line);
            }

            referenceReader.Close();

            while ((line = validReader.ReadLine()) != null)
            {
                validKeyWords.Add(line);
            }

            validReader.Close();

        }

        public bool FindKeyWords(String fileName, String article, int i, MySqlConnection connection)
        {
            bool hasKeyword = false;
            foreach (String word in validKeyWords)
            {
                
                if (article.Contains(word.Trim()) && word.Trim() != "" && word != null)
                {
                    if (!hasKeyword)
                        Console.WriteLine(i + ") Success\n\tFile: " + fileName + "\n\tKeywords:");
                    Console.WriteLine("\t\t" + word);
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO keywords(keyword, fileName) VALUES(?keyword, ?fileName)";
                    cmd.Parameters.AddWithValue("?keyword", word);
                    cmd.Parameters.AddWithValue("?fileName", fileName);
                    cmd.ExecuteNonQuery();
                    hasKeyword = true;
                }
            }
            return hasKeyword;
        }

        public bool PassesAllFilters(String article)
        {
            if (IsReferenceArticle(article))
                return IsValidArticle(article);
            return false;
        }

        private bool IsReferenceArticle(String article)
        {
            foreach(String word in referenceKeyWords)
            {
                if (article.Contains(word))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidArticle(String article)
        {
            foreach (String word in validKeyWords)
            {
                if (article.Contains(word))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
