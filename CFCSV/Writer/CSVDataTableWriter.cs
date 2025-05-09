﻿using CFCSV.Models;
using System.Data;
using System.Text;

namespace CFCSV.Writer
{
    /// <summary>
    /// Writes DataTable to CSV
    /// </summary>
    public class CSVDataTableWriter
    {
        /// <summary>
        /// Writes DataTable to CSV
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="csvSettings"></param>
        public void Write(DataTable dataTable, CSVSettings csvSettings)
        {            
            var isWriteHeader = !System.IO.File.Exists(csvSettings.Filename);

            using (StreamWriter writer = new StreamWriter(File.OpenWrite(csvSettings.Filename), csvSettings.Encoding))
            {
                // Write headers               
                if (isWriteHeader)
                {
                    StringBuilder line = new StringBuilder("");
                    for (int columnIndex = 0; columnIndex < csvSettings.Columns.Count; columnIndex++)
                    {
                        if (line.Length > 0)
                        {
                            line.Append(csvSettings.Delimiter);
                        }
                        line.Append(csvSettings.Columns[columnIndex].Name);
                    }
                    writer.WriteLine(line.ToString());
                }

                // Write rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    string rowString = GetRowFromDataTable(dataTable.Rows[rowIndex], csvSettings);
                    writer.WriteLine(rowString);
                }
                writer.Flush();
                writer.Close();
            }
        }

        private string GetRowFromDataTable(DataRow dataRow, CSVSettings csvSettings)
        {
            StringBuilder line = new StringBuilder("");
            for (int columnIndex = 0; columnIndex < csvSettings.Columns.Count; columnIndex++)
            {
                var column = csvSettings.Columns[columnIndex];
                if (line.Length > 0)
                {
                    line.Append(csvSettings.Delimiter);
                }

                string columnValueString = GetColumnValue(dataRow[columnIndex], column);
                line.Append(columnValueString);
            }
            return line.ToString();
        }

        private string GetColumnValue(object value, CSVColumn csvColumn)
        {
            char quotes = '"';
            string valueString = null;
            if (value == null)
            {
                return csvColumn.NullValueString;
            }
            if (!string.IsNullOrEmpty(csvColumn.FormatString))
            {
                valueString = string.Format(csvColumn.FormatString, value);
            }
            else
            {
                valueString = value.ToString();
            }

            if (csvColumn.ValueQuoted)
            {
                valueString = quotes.ToString() + valueString + quotes.ToString();
            }
            return valueString;

        }
    }
}
