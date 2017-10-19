using System;



namespace ArticleAnalyzer
{
    public class Filter
    {

        public ArrayList<String> refernceKeyWords;

        public ArrayList<String> validKeyWords;

        public Filter(String referenceKeyWordsPath, String validKeyWordsPath)
        {
            FileInfo referenceKeyWordFile = new FileInfo(referenceKeyWordsPath);
            var referenceReader = file.OpenRead();

            FileInfo validKeyWordFile = new FileInfo(validKeyWordsPath);
            var validReader = file.OpenRead();

            String line = new String();

            refernceKeyWords = new ArrayList<String>();
            validKeyWords = new ArrayList<String>();

            while (line = referenceReader.Readline)
            {
                refernceKeyWords.add(line);
            }

            referenceReader.Close();

            while (line = validReader.Readline)
            {
                validKeyWords.add(line);
            }

            validReader.Close();

        }

        private bool isReferenceArticle()
        {

        }

        private bool isValidArticle()
        {

        }
    }
}
