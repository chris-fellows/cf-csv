using System.Text;

namespace CFCSV.Models
{
    public class CSVSettings
    {
        public string Filename { get; set; }
        public Char Delimiter { get; set; }
        //public bool DefaultColumnHeaderQuoted = false;              // Default header quoted status if not specified at column level
        //public bool DefaultColumnValueQuoted = false;               // Default value quoted status if not specified at column level          
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

        //public bool IsColumnHeaderQuoted(int columnIndex)
        //{
        //    return (Columns.Count == 0) ? DefaultColumnHeaderQuoted : Columns[columnIndex].HeaderQuoted;
        //}

        //public bool IsColumnValueQuoted(int columnIndex)
        //{
        //    return (Columns.Count == 0) ? DefaultColumnValueQuoted : Columns[columnIndex].ValueQuoted;
        //}

        //public void SetColumnValueQuoted(DataTable data)
        //{
        //    //ColumnValueQuotedList.Clear();

        //    Type[] noQuoteTypes = { typeof(Boolean), typeof(Byte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(Double), typeof(float), typeof(Single), typeof(DateTime) };

        //    for (int index = 0; index < data.Columns.Count; index++)
        //    {
        //        Type columnType = data.Columns[index].DataType;
        //        if (Array.IndexOf(noQuoteTypes, columnType) == -1)
        //        {
        //            Columns[index].ValueQuoted = true;
        //        }
        //        else
        //        {
        //            Columns[index].ValueQuoted = false;
        //        }
        //    }
        //}
    }
}
