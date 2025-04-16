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
        /// 'Reads CSV and returns a DataTable
        /// </summary>
        /// <param name="csvSettings"></param>
        /// <returns></returns>
        public DataTable Read(CSVSettings csvSettings)
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
                while (!reader.EndOfStream)
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
    }
}
