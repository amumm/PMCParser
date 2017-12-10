using Microsoft.AspNetCore.Mvc;
using System;
using Processing;
using Processing.Data;
using Processing.DataAnalysis;
using Processing.DataOutput;
using System.Diagnostics;

namespace PMCParser.Controllers
{
    [Route("api/[controller]")]
    public class PMController : Controller
    {
        public String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\Config_Files\config.txt";

        [HttpGet("[action]")]
        public Boolean  AnalyzeArticles()
        {
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Debug.WriteLine("Conncted Successfully");
                var results = Analyzer.Control(config, dbc);
                dbc.Close();
                return false;
            }

            Debug.WriteLine("Connection Failed");
            return true;
            //return results;
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
