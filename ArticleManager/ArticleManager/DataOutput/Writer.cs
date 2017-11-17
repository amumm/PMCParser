using ArticleAnalyzer.Data;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;

namespace ArticleAnalyzer.DataOutput
{
    static class Writer
    {
        public static void Control(Configuration config, DBConnection dbc, String outputFileName)
        {
            WriteToExcel(config, dbc, outputFileName);
        }

        private static void WriteToExcel(Configuration config, DBConnection dbc, String outputFileName)
        {
            StringBuilder sb = new StringBuilder();

            string query = "SELECT keyword, fileName FROM keywords";
            var cmd = new MySqlCommand(query, dbc.Connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string keyword = reader.GetString(0);
                string filename = reader.GetString(1);
                if (keyword != null && filename != null)
                {
                    var newLine = $"{keyword}, {filename}";
                    sb.AppendLine(newLine);

                }
                //Console.WriteLine("Keyword: " + keyword + ", File: " + filename);
            }

            File.WriteAllText(config.ExcelOutputDirectory + outputFileName, sb.ToString());
        }
    }
}
