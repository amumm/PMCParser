using PMCParserSetup.Data;
using System;
using System.Collections.Generic;

namespace PMCParserSetup.DataRetrieval
{
    public static class Retriever
    {
        public static void Control(Configuration config, DBConnection dbc)
        {
            IdManager.GetPmcIds(config.QueriesFilePath, config.IdOutputPath, config.ESearchScriptPath, config.BashPath);
            GetHtml(config, dbc);
        }

        private static void GetHtml(Configuration config, DBConnection dbc)
        {
            HtmlDownloader.Add_IDs_To_Articles_To_Analyze(config.IdOutputPath, dbc);
            HtmlDownloader.Add_IDs_To_Articles_To_Download(config.IdOutputPath, dbc);
            HtmlDownloader.UpdateDbWithStoredArticles(dbc, config.ArticleDirectory);
            HtmlDownloader.DownloadArticleByPMCId(dbc, config.ArticleDirectory, config.PMCUrlPrefix);

        }
    }
}
