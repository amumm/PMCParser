using Microsoft.AspNetCore.Mvc;
using System;
using Processing;
using Processing.Data;
using Processing.DataRetrieval;
using Processing.DataAnalysis;
using Processing.DataOutput;

namespace PMCParser.Controllers
{
    [Route("api/[controller]")]
    public class PMCController : Controller
    {

        [HttpGet("[action]")]
        public void CreateList()
        {
            Console.WriteLine("here");
            String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\config.txt";
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Console.WriteLine("Conncted Successfully");
                Retriever.Control(config, dbc);
                dbc.Close();
            }
            else Console.WriteLine("Connection Failed");
        }

        [HttpGet("[action]")]
        public void DownloadArticles()
        {
            String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\config.txt";
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Console.WriteLine("Conncted Successfully");
                Retriever.Control(config, dbc);
                dbc.Close();
            }
            else Console.WriteLine("Connection Failed");
        }

        [HttpGet("[action]")]
        public void AnalyzeArticles()
        {
            String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\config.txt";
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Console.WriteLine("Conncted Successfully");
                Analyzer.Control(config, dbc);
                dbc.Close();
            }
            else Console.WriteLine("Connection Failed");
        }

        [HttpGet("[action]")]
        public void ExportData()
        {
            String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\config.txt";
            Configuration config = new Configuration(configPath);
            String excelOutputName = "test.csv";
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Console.WriteLine("Conncted Successfully");
                Writer.Control(config, dbc, excelOutputName);
                dbc.Close();
            }
            else Console.WriteLine("Connection Failed");
        }

    }
}
