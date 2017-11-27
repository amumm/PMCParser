using Processing.Data;
using System;
using System.Collections.Generic;

namespace Processing.DataRetrieval
{
    public static class Retriever
    {
        public static void Control(Configuration config, DBConnection dbc, String Command)
        {
            if (Command.Equals("createList"))
            {
                HtmlDownloader.Add_IDs_To_Articles_To_Analyze(config.IdOutputPath, dbc);
                HtmlDownloader.Add_IDs_To_Articles_To_Download(config.IdOutputPath, dbc);
                HtmlDownloader.UpdateDbWithStoredArticles(dbc, config.ArticleDirectory);
            }

            else
                HtmlDownloader.DownloadArticleByPMCId(dbc, config.ArticleDirectory, config.PMCUrlPrefix);
        }

    }
}
