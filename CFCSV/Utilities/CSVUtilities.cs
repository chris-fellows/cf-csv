using CFCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.Utilities
{
    internal class CSVUtilities
    {
        /// <summary>
        /// Gets column values from line
        /// </summary>
        /// <param name="line"></param>
        /// <param name="csvSettings"></param>
        /// <returns></returns>
        public static List<object> GetColumnValues(string line, CSVSettings csvSettings)
        {
            List<string> valueStrings = line.Split(csvSettings.Delimiter).ToList();
            List<object> columnValues = new List<object>();

            for (int columnIndex = 0; columnIndex < csvSettings.Columns.Count; columnIndex++)
            {
                CSVColumn column = csvSettings.Columns[columnIndex];
                string valueString = valueStrings[columnIndex];
                object columnValue = null;

                if (valueString == column.NullValueString)
                {
                    columnValue = null;
                }
                else
                {
                    if (column.ValueQuoted && !String.IsNullOrEmpty(valueString))
                    {
                        valueString = valueString.Substring(1, valueString.Length - 2);
                    }

                    if (String.IsNullOrEmpty(column.FormatString))
                    {
                        columnValue = TypeUtilities.ConvertValueToType(valueString, column.DataType);
                    }
                    else    // Decode using format
                    {
                        // TODO: Fix to use format (E.g. Dates)
                        columnValue = TypeUtilities.ConvertValueToType(valueString, column.DataType);
                    }
                }
                columnValues.Add(columnValue);
            }

            return columnValues;
        }

        public static List<string> GetColumnValueStrings(string line, char delimiter)
        {
            // TODO: Improve this
            return line.Split(delimiter).ToList();
        }
    }
}
