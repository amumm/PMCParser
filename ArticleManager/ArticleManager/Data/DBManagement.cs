using Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleAnalyzer.Data
{
    class DBManagement
    {
        public static void AddDataTypesToTable(String dataTypesFilePath, DBConnection dbc)
        {
            FileInfo dataTypes = new FileInfo(dataTypesFilePath);
            var dataTypesReader = dataTypes.OpenText();

            MySqlCommand cmd;
            String line = "";
            String keyword = "";
            String datatype = "";
            String[] datatypeAndKeyword;

            while ((line = dataTypesReader.ReadLine()) != null)
            {
                datatypeAndKeyword = line.Split(':');
                datatype = datatypeAndKeyword[0];
                keyword = datatypeAndKeyword[1];

                cmd = dbc.Connection.CreateCommand();
                cmd.CommandText =
                    @"INSERT INTO Data_Types(Key_Word, Data_Type) 
                            SELECT ?keyword, ?datatype FROM dual
                                WHERE NOT EXISTS(SELECT 1  FROM Data_Types
                                    WHERE Key_Word = ?keyword
                                    AND Data_Type = ?datatype)";
                cmd.Parameters.AddWithValue("?keyword", keyword);
                cmd.Parameters.AddWithValue("?datatype", datatype);
                cmd.ExecuteNonQuery();
            }

            dataTypesReader.Close();
        }


    }
}
