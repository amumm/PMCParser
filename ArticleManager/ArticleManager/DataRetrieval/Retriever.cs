using ArticleAnalyzer.Data;
using System;

namespace ArticleAnalyzer.DataRetrieval
{
    public static class Retriever
    {
        public static void Control(Configuration config, DBConnection dbc)
        {
            GetHtml(config, dbc);
            String[] query = { "opsin gene expression" };
            IdManager.GetPmcIds(query, config.IdOutputPath, config.ESearchScriptPath, config.BashPath);
        }

        private static void GetHtml(Configuration config, DBConnection dbc)
        {
            //HtmlDownloader.AddArticlesToDownloadToDb(config.IdOutputPath, dbc);
            //HtmlDownloader.UpdateDbWithStoredArticles(dbc, config.ArticleDirectory);
            HtmlDownloader.DownloadArticleByPMCId(dbc, config.ArticleDirectory, config.PMCUrlPrefix);

        }
    }
}
