using CFCSV.Models;
using CFCSV.Utilities;

namespace CFCSV.Reader
{
    /// <summary>
    /// Base class for CSV reader
    /// </summary>
    public abstract class CSVReaderBase
    {
        /// <summary>
        /// Gets CSV settings (if any) from file. Identifies basic info such as delimiter, encoding, column headers.
        /// 
        /// Note that only basic column properties are set (Name, DataType). It doesn't analyse the data rows to try and guess
        /// the other properties.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public CSVSettings? GetSettings(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("Files does not exist", file);
            }

            CSVSettings csvSettings = new CSVSettings();
                       
            using (StreamReader reader = new StreamReader(file))
            {                
                const Char quotes = '"';
                                
                if (!reader.EndOfStream)
                {                    
                    // Read header
                    var line = reader.ReadLine();

                    if (!String.IsNullOrEmpty(line))
                    { 
                        // Get delimiter
                        var delimiter = CSVUtilities.GetDelimiter(line);
                        if (delimiter != null)
                        {
                            // Get columns
                            var headers = CSVUtilities.GetColumnValueStrings(line, delimiter.Value, quotes);

                            csvSettings.Delimiter = delimiter.Value;
                            csvSettings.Encoding = reader.CurrentEncoding;    // Best guess
                            csvSettings.Columns = headers.Select(header =>
                            {
                                return new CSVColumn()
                                {
                                    Name = header,
                                    DataType = typeof(String)
                                };
                            }).ToList();                            
                        }
                    }                    
                }
            }

            return csvSettings.Columns.Any() ? csvSettings : null;
        }            
    }
}
