using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleAnalyzer.dataMining
{
    public class JournalPaper
    {
        public string JournalTitle { get; set; }

        public string PaperTitle { get; set; }

        public string Issue { get; set; }

        public string Volume { get; set; }

        public string Date { get; set; }

        public Array Authors;

        public Array CorrespondingAuthors;

        public Array dataTypes;

        public JournalPaper()
        {

        }

        public void AddAttribute(String attribute, String content)
        {

            switch (attribute)
            {
                case "journal_title":
                    JournalTitle = content;
                    break;
                case "title":
                    PaperTitle = content;
                    break;
                case "authors":
                    Authors = content.Split(',');
                    break;
                case "date":
                    Date = content;
                    break;
                case "issue":
                    Issue = content;
                    break;
                case "volume":
                    Volume = content;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }
 
    }
}
