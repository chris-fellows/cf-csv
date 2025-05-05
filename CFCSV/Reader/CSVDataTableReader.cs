using CFCSV.Models;
using CFCSV.Utilities;
using System.Data;

namespace CFCSV.Reader
{
    /// <summary>
    /// Reads CSV in to DataTable
    /// </summary>
    public class CSVDataTableReader
    {
        /// <summary>
        /// Reads CSV and returns a DataTable. Requires csvSettings to know the columns.
        /// </summary>
        /// <param name="csvSettings"></param>
        /// <returns></returns>
        public DataTable Read(CSVSettings csvSettings, int? maxDataRows = null)
        {
            DataTable dataTable = new DataTable();

            // Set column s
            for (int columnIndex = 0; columnIndex < csvSettings.Columns.Count; columnIndex++)
            {
                CSVColumn csvColumn = csvSettings.Columns[columnIndex];
                dataTable.Columns.Add(csvColumn.Name, csvColumn.DataType);
            }
            
            using (StreamReader reader = new StreamReader(csvSettings.Filename))
            {
                int lineCount = 0;
                var gotAllRequiredRows = false;
                while (!reader.EndOfStream &&
                    !gotAllRequiredRows)
                {
                    lineCount++;

                    // Read line
                    string line = reader.ReadLine();

                    if (lineCount > 1)
                    {
                        // Get column values
                        List<object> columnValues = CSVUtilities.GetColumnValues(line, csvSettings);

                        // Create row
                        DataRow dataRow = dataTable.NewRow();
                        CreateDataRow(csvSettings, dataRow, columnValues);
                        dataTable.Rows.Add(dataRow);
                    }

                    // Abort if row line
                    if (maxDataRows != null && lineCount - 1 >= maxDataRows) gotAllRequiredRows = true;
                }
            }
            return dataTable;
        }

        private void CreateDataRow(CSVSettings csvSettings, DataRow dataRow, List<object> columnValues)
        {
            for (int columnIndex = 0; columnIndex < csvSettings.Columns.Count; columnIndex++)
            {
                CSVColumn csvColumn = csvSettings.Columns[columnIndex];
                dataRow[columnIndex] = columnValues[columnIndex];
            }
        }

        private void CreateDataRow(DataRow dataRow, List<string> columnValues)
        {
            for (int columnIndex = 0; columnIndex < columnValues.Count; columnIndex++)
            {                
                dataRow[columnIndex] = columnValues[columnIndex];
            }
        }

        /// <summary>
        /// Reads CSV and returns a DataTable
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public DataTable Read(string file, int? maxDataRows = null)
        {
            DataTable dataTable = new DataTable();

            using (StreamReader reader = new StreamReader(file))
            {
                int lineCount = 0;
                Char? delimiter = null;
                Char quotes = '"';
                var gotAllRequiredRows = false;
                while (!reader.EndOfStream &&
                    !gotAllRequiredRows)
                {
                    lineCount++;

                    // Read line
                    string line = reader.ReadLine();

                    if (lineCount == 1)    // Header
                    {
                        delimiter = CSVUtilities.GetDelimiter(line);

                        // Add column headers
                        var columnValues = CSVUtilities.GetColumnValueStrings(line, delimiter.Value, quotes);
                        foreach(var columnValue in columnValues)
                        {
                            dataTable.Columns.Add(columnValue, typeof(String));
                        }
                    }
                    else    // Row
                    {
                        // Get column values
                        var columnValues = CSVUtilities.GetColumnValueStrings(line, delimiter.Value, quotes);

                        // Create row
                        DataRow dataRow = dataTable.NewRow();
                        CreateDataRow(dataRow, columnValues);
                        dataTable.Rows.Add(dataRow);
                    }

                    // Abort if row line
                    if (maxDataRows != null && lineCount - 1 >= maxDataRows) gotAllRequiredRows = true;                   
                }
            }

            return dataTable;
        }
    }
}
