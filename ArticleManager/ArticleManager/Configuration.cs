using System;
using System.Collections.Generic;
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

            var equals = configString.AllIndexesOf("=");
            var semiColon = configString.AllIndexesOf(";");
            List<String> parameters = new List<String>();

            for(var i = 0; i < equals.Count; i++)
            {
                var length = semiColon[i] - equals[i] - 4;
                parameters.Add(configString.Substring(equals[i] + 3, length));
            }

            foreach (var param in parameters) Console.WriteLine(param);

            DataBaseConnectionName = parameters[0];
            DataBaseName = parameters[1];
            DataBaseUsername = parameters[2];
            DataBasePassword = parameters[3];
            BashPath = parameters[4];
            ESearchScriptPath = parameters[5];
            IdOutputPath = parameters[6];
            PMCUrlPrefix = parameters[7];
            ArticleDirectory = parameters[8];
            ExcelOutputDirectory= parameters[9];


        }

        
    }
}
