using ArticleAnalyzer.DataRetrieval;
using ArticleAnalyzer.DataAnalysis;
using ArticleAnalyzer.DataOutput;
using ArticleAnalyzer.Data;
using System;

namespace ArticleAnalyzer
{
    class Control
    {
        static void Main(string[] args)
        {
            String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\config.txt";
            Configuration config = new Configuration(configPath);
            String excelOutputName = "test.csv";
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Console.WriteLine("Conncted Successfully");
                //Retriever.Control(config, dbc);
                Analyzer.Control(config, dbc);
                Writer.Control(config, dbc, excelOutputName);
                dbc.Close();
            }
            else Console.WriteLine("Connection Failed");
        }
    }
}
