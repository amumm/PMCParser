using System;
using PMCParserSetup.Data;
using PMCParserSetup.DataRetrieval;

namespace PMCParserSetup
{
    class Control
    {
        static void Main(string[] args)
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
    }
}
