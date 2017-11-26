using MySql.Data.MySqlClient;
using Processing.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Processing.DataAnalysis
{
    public class ArticleParser
    {
        private FileInfo file;

        private String article;

        private ArrayList referenceKeyWords;

        private ArrayList validKeyWords;

        private DBConnection dbc;

        private JournalPaper paper;

        public ArticleParser(FileInfo file, String article, DBConnection dbc, JournalPaper paper)
        {
            this.file = file;
            this.article = article;
            this.dbc = dbc;
            this.paper = paper;

            referenceKeyWords = new ArrayList();
            validKeyWords = new ArrayList();

            string query = "SELECT Key_Word FROM Data_Types";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var validReader = cmd.ExecuteReader();
            while (validReader.Read())
            {
                validKeyWords.Add(validReader.GetString(0));
            }
            validReader.Close();

            query = "SELECT keyword FROM Reference_Keywords";
            cmd = new MySqlCommand(query, dbc.Connection);
            var referenceReader = cmd.ExecuteReader();
            while (referenceReader.Read())
            {
                referenceKeyWords.Add(referenceReader.GetString(0));
            }
            referenceReader.Close();
        }

        public bool PassesAllFilters()
        {
            foreach (String word in referenceKeyWords)
            {
                if (article.Contains(word))
                {
                    Console.WriteLine("Failed:\n\tReason: Failed Filter\n\tFile: " + file.Name);
                    return false;
                }
            }

            return true;
        }

        public String DefineSections()
        {
            String temp = article;
            String result = "";

            if (temp.Contains("Methods") || temp.Contains("Results"))
            {
                List<int> indexesOfSections = article.AllIndexesOf("head no_bottom_margin");

                for (var i = 0; i < indexesOfSections.Count() - 1; i++)
                {
                    int length = indexesOfSections.ElementAt(i + 1) - indexesOfSections.ElementAt(i);
                    temp = article.Substring(indexesOfSections.ElementAt(i), length);
                    if (temp.Contains("Methods") || temp.Contains("Results"))
                    {
                        result += temp + "\t\n ";

                    }
                }
            }

            return result;
        }

        public void FindHeaderInfo()
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

                FindContent(tag, tagLength, attribute);
            }
            Console.WriteLine("");
        }

        public void FindKeyWords(String fileName, String article, int i, MySqlConnection connection)
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
                    cmd.CommandText =
                        @"INSERT INTO keywords(keyword, fileName) 
                            SELECT ?keyword, ?fileName FROM dual
                                WHERE NOT EXISTS(SELECT 1  FROM keywords
                                    WHERE keyword = ?keyword
                                    AND fileName = ?fileName)";
                    cmd.Parameters.AddWithValue("?keyword", word);
                    cmd.Parameters.AddWithValue("?fileName", fileName);
                    cmd.ExecuteNonQuery();
                    hasKeyword = true;
                }
            }

            if(!hasKeyword)
                Console.WriteLine("Failed:\n\tReason: Neither Methods nor Results Contain Keywords\n\tFile: " + file.Name);
        }


        private void FindContent(String tag, int tagLength, String attribute)
        {
            int start, end;
            String content = "";
            if (article.Contains(tag))
            {
                start = article.IndexOf(tag);
                content = article.Substring(start + tagLength + 11); //move foraward to the start of content
                end = content.IndexOf("/");
                content = content.Substring(0, end - 2);
                Console.WriteLine(content);
                paper.AddAttribute(attribute, content);

            }
        }
    }
}
