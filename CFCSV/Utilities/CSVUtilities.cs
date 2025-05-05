using CFCSV.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.Utilities
{
    public static class CSVUtilities
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

        public static string RemoveOuterQuotes(string line, Char quotes)
        {
            if (line.StartsWith(quotes) && line.EndsWith(quotes))
            {
                return line.Substring(1, line.Length - 2);
            }
            return line;
        }

        public static string EscapeQuotes(string line, Char quotes)
        {
            return line.Contains(quotes) ?
                line.Replace($"{quotes}", $"{quotes}{quotes}") :
                line;
        }

        /// <summary>
        /// Gets column value strings from line.
        /// 
        /// Example with quotes in values: Column 1,"Column 2""XXX",Column 3
        /// </summary>
        /// <param name="line"></param>
        /// <param name="delimiter"></param>
        /// <param name="quotes"></param>
        /// <returns></returns>
        public static List<string> GetColumnValueStrings(string line, Char delimiter, Char quotes)
        {
            var columnValues = new List<string>();            

            // Process each character.
            // We track active quotes to handle escaped quotes & delimiters inside a string.
            var columnValue = new StringBuilder("");
            short countActiveQuotes = 0;
            for (int charIndex = 0; charIndex < line.Length; charIndex++)
            {                
                bool isAppendCharToColumnValue = true;      // Default

                // Get current char
                Char currentChar = line[charIndex];                                

                if (currentChar == delimiter)    // Delimiter
                {   
                    // Delimiter may be a column delimiter or inside a quoted string. Quoted string can also contain escaped quotes too and we
                    // determine this based on the count of active quotes. If we have an even number of double quotes then we know that the
                    // quoted string has ended.
                    if (countActiveQuotes > 0)    // Quotes are active
                    {                        
                        if (countActiveQuotes % 2 == 0)   // End quotes
                        {
                            isAppendCharToColumnValue = false;

                            // Add column to list, start new column
                            if (columnValue.Length > 0)
                            {
                                columnValues.Add(columnValue.ToString());
                                columnValue.Length = 0;                             
                                countActiveQuotes = 0;
                            }
                        }
                        else    
                        {
                            // No action, delimiter is part of a quoted string
                        }
                    }
                    else     // Delimiter as there are no active quotes
                    {
                        isAppendCharToColumnValue = false;

                        // Add column to list, start new column
                        if (columnValue.Length > 0)
                        {
                            columnValues.Add(columnValue.ToString());
                            columnValue.Length = 0;                            
                            countActiveQuotes = 0;
                        }
                    }                    
                }
                else if (currentChar == quotes)   // Quotes
                {                 
                    if (columnValue.Length == 0)   // Start of new column
                    {                        
                        countActiveQuotes = 1;
                    }
                    else    // Either end of column or escaped quotes within column
                    {
                        countActiveQuotes++;
                    }                    
                }
                
                if (isAppendCharToColumnValue)
                {
                    columnValue.Append(currentChar);
                }
            }

            // Add final column
            if (columnValue.Length > 0)
            {
                columnValues.Add(columnValue.ToString());
            }

            return columnValues;
        }

        /// <summary>
        /// Gets delimiter for line
        /// 
        /// It's recommended to just call this against the header because row lines might contain these characters within
        /// quoted values.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Char? GetDelimiter(string line)
        {
            List<Char> delimiters = new List<char>() { (Char)9, ',', '|', '#', ';' };
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
    }
}
