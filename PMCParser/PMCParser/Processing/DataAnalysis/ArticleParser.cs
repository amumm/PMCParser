using MySql.Data.MySqlClient;
using Processing.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Processing.DataAnalysis
{
    public class ArticleParser
    {
        private FileInfo file;

        private String article;

        private ArrayList referenceKeyWords;

        private List<DataType> dataTypes;

        private DBConnection dbc;

        private JournalPaper paper;

        public ArticleParser(FileInfo file, String article, DBConnection dbc, JournalPaper paper)
        {
            this.file = file;
            this.article = article;
            this.dbc = dbc;
            this.paper = paper;

            referenceKeyWords = new ArrayList();
            dataTypes = new List<DataType>();

            string query = "SELECT DISTINCT Data_Type FROM Data_Types";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var typeReader = cmd.ExecuteReader();
            List<String> types = new List<String>();
            while (typeReader.Read())
            {
                types.Add(typeReader.GetString(0));
            }

            typeReader.Close();

            foreach (var type in types)
            {
                var temp = new DataType(type);
                dataTypes.Add(temp);

                query = @"SELECT Key_Word FROM Data_Types
                          WHERE Data_Type = '" + temp.Name + "'";
                cmd = new MySqlCommand(query, dbc.Connection);
                var keyWordReader = cmd.ExecuteReader();
                while (keyWordReader.Read())
                {
                    temp.Keywords.Add(keyWordReader.GetString(0));
                }
                keyWordReader.Close();
            }

            query = "SELECT keyword FROM Reference_Keywords";
            cmd = new MySqlCommand(query, dbc.Connection);
            var referenceReader = cmd.ExecuteReader();
            while (referenceReader.Read())
            {
                referenceKeyWords.Add(referenceReader.GetString(0));
            }
            referenceReader.Close();
        }

        public bool PassesAllFilters(int i)
        {
            MySqlCommand updateCommand = dbc.Connection.CreateCommand();

            if (article.Contains("sub-head section-title"))
            {
                Console.Write(i + ") Failed: PDF, ");

                file.Delete();

                updateCommand = dbc.Connection.CreateCommand();
                updateCommand.CommandText =
                @"UPDATE Article_Status s
                    SET Downloaded = 0, Valid = 0, To_Analyze = '0'
                    WHERE s.PMC_Id = '" + file.Name + "'";
                updateCommand.ExecuteNonQuery();
                Console.WriteLine("the article " + file.Name + " was deleted\n");

                return false;
            }

            foreach (String word in referenceKeyWords)
            {
                if (article.ToUpper().Contains(word.ToUpper()))
                {
                    Console.Write(i + ") Failed: Reference Article, ");

                    file.Delete();

                    updateCommand = dbc.Connection.CreateCommand();
                    updateCommand.CommandText =
                    @"UPDATE Article_Status s
                        SET Downloaded = 0, Valid = 0, To_Analyze = '0'
                        WHERE s.PMC_Id = '" + file.Name + "'";
                    updateCommand.ExecuteNonQuery();
                    Console.WriteLine("the article " + file.Name + " was deleted\n");

                    return false;
                }

            }

            return true;
        }

        public String DefineSections(int j)
        {
            String temp = article;
            String result = "";

            if (temp.ToUpper().Contains("METHODS") || temp.ToUpper().Contains("RESULTS"))
            {
                List<int> indexesOfSections = article.AllIndexesOf("head no_bottom_margin");

                for (var i = 0; i < indexesOfSections.Count() - 1; i++)
                {
                    int length = indexesOfSections.ElementAt(i + 1) - indexesOfSections.ElementAt(i);
                    temp = article.Substring(indexesOfSections.ElementAt(i), length);
                    if (temp.ToUpper().Contains("METHODS") || temp.ToUpper().Contains("RESULTS"))
                    {
                        result += temp + "\t\n ";

                    }
                }
            }

            if(result == null || result == "" || result == " ")
            {
                MySqlCommand updateCommand = dbc.Connection.CreateCommand();

                Console.Write(j + ") Failed: No Methods or Results ");

                file.Delete();

                updateCommand = dbc.Connection.CreateCommand();
                updateCommand.CommandText =
                @"UPDATE Article_Status s
                        SET Downloaded = 0, Valid = 0, To_Analyze = '0'
                        WHERE s.PMC_Id = '" + file.Name + "'";
                updateCommand.ExecuteNonQuery();
                Console.WriteLine("the article " + file.Name + " was deleted\n");

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
            //Console.WriteLine("");
        }

        public void FindKeyWords(String fileName, String article, int i, MySqlConnection connection)
        {
            bool hasKeyword = false;
            bool hasDataType = false;
            String result = "";
            foreach (var type in dataTypes)
            {
                DataType temp = new DataType(type.Name);
                paper.DataTypes.Add(temp);

                foreach (String word in type.Keywords)
                {
                    if (article.Contains(word.Trim()) && word.Trim() != "" && word != null)
                    {
                        if (!hasKeyword) result += i + ") Success File: " + fileName + " contains the data types and keywords: ";

                        if (!hasDataType)
                        {
                            result += type.Name + ": ";
                        }

                        result += word + ", ";
                        temp.Keywords.Add(word);
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
                        hasDataType = true;
                    }
                }
                hasDataType = false;
            }

            if(!hasKeyword)
                Console.WriteLine(i + ") Failed: Neither Methods nor Results Contain Keywords in File: " + file.Name);

            Console.WriteLine(result.Substring(0, result.Length - 2) + "\n");
        }


        private void FindContent(String tag, int tagLength, String attribute)
        {
            int start, end;
            String content = "";
            if (article.Contains(tag))
            {
                start = article.IndexOf(tag);
                content = article.Substring(start + tagLength + 11); //move foraward to the start of content
                end = content.IndexOf(">")  - 1;
                content = content.Substring(0, end);
                //Console.WriteLine(content);
                paper.AddAttribute(attribute, content);

            }
        }
    }
}
