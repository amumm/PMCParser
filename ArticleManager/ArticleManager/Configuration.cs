using System;
using System.IO;

namespace ArticleAnalyzer
{
    public class Configuration
    {
        public String DataBaseConnectionName;

        public String DataBaseName;

        public String DataBaseUsername;

        public String DataBasePassword;

        public String BashPath;

        public String ESearchScriptPath;

        public String IdOutputPath;

        public String PMCUrlPrefix;

        public String ArticleDirectory;

        public String ExcelOutputDirectory;

        public Configuration(String ConfigFilePath)
        {
            FileInfo config = new FileInfo(ConfigFilePath);
            StreamReader reader = config.OpenText();
            String configString = reader.ReadToEnd();


        }

        
    }
}
