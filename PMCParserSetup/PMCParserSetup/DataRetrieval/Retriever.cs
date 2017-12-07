using PMCParserSetup.Data;
using System;
using System.Collections.Generic;

namespace PMCParserSetup.DataRetrieval
{
    public static class Retriever
    {
        public static void Control(Configuration config, DBConnection dbc)
        {
            while (true)
            {
                Console.Write("\n1) Fetch IDs\n2) Download Articles by IDs\n3) Main Menu\nEnter a number:");
                var input = Console.ReadLine();
                if(input == "1"){
                    IdManager.GetPmcIds(config.QueriesFilePath, config.IdOutputPath, config.ESearchScriptPath, config.BashPath);
                    HtmlDownloader.Add_IDs_To_Articles_Status(config.IdOutputPath, dbc);
                    HtmlDownloader.UpdateDbWithStoredArticles(dbc, config.ArticleDirectory);
                }
                else if(input == "2"){
                    HtmlDownloader.DownloadArticleByPMCId(dbc, config.ArticleDirectory, config.PMCUrlPrefix);
                }
                if(input == "3") {
                    break;
                }
                input = "0";
            }
        }
    }
}
