using Microsoft.AspNetCore.Mvc;
using System;
using Processing;
using Processing.Data;
using Processing.DataRetrieval;
using Processing.DataAnalysis;
using Processing.DataOutput;
using System.Diagnostics;

namespace PMCParser.Controllers
{
    [Route("api/[controller]")]
    public class PMController : Controller
    {
        public String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\config.txt";

        [HttpGet("[action]")]
        public void CreateList()
        {
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Debug.WriteLine("Conncted Successfully");
                Retriever.Control(config, dbc, "createList");
                dbc.Close();
            }
            else Debug.WriteLine("Connection Failed");
        }

        [HttpGet("[action]")]
        public void DownloadArticles()
        {
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Debug.WriteLine("Conncted Successfully");
                Retriever.Control(config, dbc, "downloadArticles");
                dbc.Close();
            }
            else Debug.WriteLine("Connection Failed");
        }

        [HttpGet("[action]")]
        public void AnalyzeArticles()
        {
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Debug.WriteLine("Conncted Successfully");
                Analyzer.Control(config, dbc);
                dbc.Close();
            }
            else Debug.WriteLine("Connection Failed");
        }

        [HttpGet("[action]")]
        public void ExportData()
        {
            Configuration config = new Configuration(configPath);
            String excelOutputName = "test.csv";
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Debug.WriteLine("Conncted Successfully");
                Writer.Control(config, dbc, excelOutputName);
                dbc.Close();
            }
            else Debug.WriteLine("Connection Failed");
        }

    }
}
