using System;
using PMCParserSetup.Data;
using PMCParserSetup.DataRetrieval;

namespace PMCParserSetup
{
    class Control
    {
        static void Main(string[] args)
        {
            String configPath = @"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\Config_Files\config.txt";
            Configuration config = new Configuration(configPath);
            DBConnection dbc = new DBConnection(config.DataBaseConnectionName, config.DataBaseName, config.DataBaseUsername, config.DataBasePassword);
            if (dbc.IsConnected())
            {
                Console.WriteLine("Connected Successfully to Database");
                while (true)
                {
                    Console.Write("\n1) ID Managment Menu\n2) Database Management\n3) Quit\nEnter a number:");
                    var input = Console.ReadLine();
                    if (input == "1"){
                        Retriever.Control(config, dbc);
                    }
                    else if (input == "2") {
                        DBManagement.AddReferenceWordsToTable(@"C:\Users\mumm9\Documents\ISU\Fall2017\COMS490\repos\PMCParser\Config_Files\referenceKeyWords.txt", dbc);
                    }
                    if (input == "3") break;
                    input = "0";
                }
                dbc.Close();
            }
            else Console.WriteLine("Connection to Database Failed");
        }
    }
}
