using CFCSV.Models;
using CFCSV.Utilities;
using System.Text;

namespace CFCSV
{
    /// <summary>
    /// Analyses CSV files
    /// </summary>
    public class CSVAnalyser
    {
        /// <summary>
        /// Reads setting for CSV
        /// 
        /// The DataType is set as String because it could be multiple possible types
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public CSVSettings ReadSettings(string filename)
        {
            CSVSettings csvSettings = new CSVSettings()
            {
                Filename = filename,
                Columns = new List<CSVColumn>(),
                Encoding = Encoding.UTF8
            };

            Char quotes = '"';
            using (StreamReader reader = new StreamReader(filename))
            {
                // Read headers
                string headerLine = reader.ReadLine();

                // Get delimiter
                Char? delimiter = GetDelimiterFromLine(headerLine);

                if (delimiter == null)
                {
                    throw new ArgumentException("File does not contain a column delimiter");
                }

                csvSettings.Delimiter = delimiter.Value;

                // Get column headers
                List<string> columnHeaders = CSVUtilities.GetColumnValueStrings(headerLine, csvSettings.Delimiter);

                // Set columns
                for (int columnIndex = 0; columnIndex < columnHeaders.Count; columnIndex++)
                {
                    CSVColumn csvColumn = new CSVColumn()
                    {
                        Name = columnHeaders[columnIndex],
                        HeaderQuoted = columnHeaders[columnIndex].StartsWith(quotes.ToString())
                    };
                    csvSettings.Columns.Add(csvColumn);
                }
            
                // Read rows
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    List<string> columnValueStrings = CSVUtilities.GetColumnValueStrings(line, csvSettings.Delimiter);
                    
                    for (int columnIndex = 0; columnIndex < csvSettings.Columns.Count; columnIndex++)
                    {
                        CSVColumn csvColumn = csvSettings.Columns[columnIndex];
                        string columnValueString = columnValueStrings[columnIndex];
                        if (columnValueString.StartsWith(quotes.ToString()))
                        {
                            csvColumn.ValueQuoted = true;                            
                        }
                    }                        
                }
                
                reader.Close();
            }
            return csvSettings;
        }

        private Char? GetDelimiterFromLine(string line)
        {
            List<Char> delimiters = new List<char>() { (Char)9, ',', '|', '#' };
            for (int index = 0; index < line.Length; index++)
            {
                int delimiterIndex = delimiters.IndexOf(line[index]);
                if (delimiterIndex != -1)
                {
                    return delimiters[delimiterIndex];
                }
            }
            return null;
        }

        ///// <summary>
        ///// Returns all possible data types for the column
        ///// </summary>
        ///// <param name="dataTable"></param>
        ///// <param name="columnIndex"></param>
        ///// <returns></returns>
        //public List<Type> GetPossibleColumnDataTypes(DataTable dataTable, int columnIndex,
        //                                            List<Type> dataTypesToConsider)
        //{
        //    List<Type> dataTypes = new List<Type>();

        //    // Get all distinct column values
        //    var columnValues = DataTableUtilities.GetDistinctValues<object>(dataTable, dataTable.Columns[columnIndex].ColumnName);

        //    foreach(var dataType in dataTypesToConsider)
        //    {
        //        bool result = TypeUtilities.CanConvertAllValuesToType(columnValues, dataType);
        //        if (result)
        //        {
        //            dataTypes.Add(dataType);
        //        }
        //    }
        //    return dataTypes;
        //}
    }
}
