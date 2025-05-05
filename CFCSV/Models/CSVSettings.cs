using System.Text;

namespace CFCSV.Models
{
    public class CSVSettings
    {
        public string Filename { get; set; }
        public Char Delimiter { get; set; }
      
        public List<CSVColumn> Columns = new List<CSVColumn>();
        public Encoding Encoding { get; set; }

        public CSVSettings()
        {

        }

        public CSVSettings(string filename, Char delimiter) //, bool defaultColumnHeaderQuoted, bool defaultColumnValueQuoted)
        {
            Filename = filename;
            Delimiter = delimiter;
            //DefaultColumnHeaderQuoted = defaultColumnHeaderQuoted;
            //DefaultColumnValueQuoted = defaultColumnValueQuoted;
        }              
    }
}
