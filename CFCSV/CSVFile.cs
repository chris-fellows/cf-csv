//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using System.IO;

//namespace CFUtilities.CSV
//{
//    /// <summary>
//    /// CSV file. 
//    /// 
//    /// TODO: Remove this, replace with CSVWriter & CSVReader
//    /// </summary>
//    public class CSVFile
//    {
//        public static void WriteHeaders(CSVSettings settings, string[] columnHeaderList)
//        {
//            if (!File.Exists(settings.Filename))
//            {
//                using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//                {
//                    WriteHeaders(settings, columnHeaderList, writer);
//                    writer.Flush();
//                    writer.Close();
//                }

//                /*
//                    StringBuilder line = new StringBuilder("");
//                for (int columnIndex = 0; columnIndex < columnHeaderList.Length; columnIndex++)
//                {
//                    line.Append(line.Length == 0 ? "" : settings.Delimiter.ToString());
//                    line.Append(FormatHeaderValue(settings, columnHeaderList[columnIndex], columnIndex));
//                }

             
//                        using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//                        {
//                            writer.WriteLine(line.ToString());
//                            writer.Flush();
//                            writer.Close();
//                        }
//                  */
             
//            }
//        }

//        private static void WriteHeaders(CSVSettings settings, string[] columnHeaderList, StreamWriter writer)
//        {
//            //if (!File.Exists(settings.Filename))
//            //{
//                StringBuilder line = new StringBuilder("");
//                for (int columnIndex = 0; columnIndex < columnHeaderList.Length; columnIndex++)
//                {
//                    line.Append(line.Length == 0 ? "" : settings.Delimiter.ToString());
//                    line.Append(FormatHeaderValue(settings, columnHeaderList[columnIndex], columnIndex));
//                }

//                writer.WriteLine(line.ToString());             

//                /*
//                using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//                {
//                    writer.WriteLine(line.ToString());
//                    writer.Flush();
//                    writer.Close();
//                }
//                */
//            //}
//        }

//        public static void WriteValues(CSVSettings settings, string[] columnValueList)
//        {
//            using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//            {
//                WriteValues(settings, columnValueList, writer);
//                //writer.WriteLine(line.ToString());
//                writer.Flush();
//                writer.Close();
//            }

//            /*
//            StringBuilder line = new StringBuilder("");
//            for (int columnIndex = 0; columnIndex < columnValueList.Length; columnIndex++)
//            {
//                line.Append(line.Length == 0 ? "" : settings.Delimiter.ToString());
//                line.Append(FormatColumnValue(settings, columnValueList[columnIndex], columnIndex));
//            }


//            using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//            {
//                writer.WriteLine(line.ToString());
//                writer.Flush();
//                writer.Close();              
//            } 
//            */           
//        }

//        private static void WriteValues(CSVSettings settings, string[] columnValueList, StreamWriter writer)
//        {
//            StringBuilder line = new StringBuilder("");
//            for (int columnIndex = 0; columnIndex < columnValueList.Length; columnIndex++)
//            {
//                line.Append(line.Length == 0 ? "" : settings.Delimiter.ToString());
//                line.Append(FormatColumnValue(settings, columnValueList[columnIndex], columnIndex));
//            }


//            //using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//            //{
//                writer.WriteLine(line.ToString());
//                //writer.Flush();
//                //writer.Close();
//            //}
//        }

//        /// <summary>
//        /// Writes the file from a DataTable
//        /// </summary>
//        /// <param name="settings"></param>
//        /// <param name="dataTable"></param>
//        public static void WriteDataTable(CSVSettings settings, DataTable dataTable, bool deleteOldFile)
//        {
//            Directory.CreateDirectory(Path.GetDirectoryName(settings.Filename));
//            if (deleteOldFile && File.Exists(settings.Filename))
//            {
//                File.Delete(settings.Filename);
//            }            

//            using (StreamWriter writer = new StreamWriter(settings.Filename, true))
//            {
//                // Write column headers
//                string[] columnHeaderList = new string[dataTable.Columns.Count];
//                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
//                {
//                    columnHeaderList[columnIndex] = dataTable.Columns[columnIndex].ColumnName;
//                }
//                WriteHeaders(settings, columnHeaderList, writer);

//                // Write column values
//                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
//                {
//                    string[] columnValueList = new string[dataTable.Columns.Count];
//                    for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
//                    {
//                        columnValueList[columnIndex] = dataTable.Rows[rowIndex][columnIndex].ToString();
//                    }
//                    WriteValues(settings, columnValueList, writer);
//                }
//                writer.Flush();
//                writer.Close();
//            }
//        }

//        private static string FormatHeaderValue(CSVSettings settings, string value, int columnIndex)
//        {
//            return settings.IsColumnHeaderQuoted(columnIndex) ? string.Format("{0}{1}{0}", (Char)'"', value) : value;
//        }

//        private static string FormatColumnValue(CSVSettings settings, string value, int columnIndex)
//        {
//            return settings.IsColumnValueQuoted(columnIndex) ? string.Format("{0}{1}{0}", (Char)'"', value) : value;
//        }

//        /// <summary>
//        /// Reads CSV in to DataTable
//        /// </summary>
//        /// <param name="settings"></param>
//        /// <param name="columnTypes"></param>
//        /// <returns></returns>
//        public DataTable Read(CSVSettings settings, List<Type> columnTypes)
//        {
//            DataTable dataTable = new DataTable();            

//            using (StreamReader reader = new StreamReader(settings.Filename))
//            {
//                int lineCount = 0;
//                string[] headers = new string[0];
//                while (!reader.EndOfStream)
//                {
//                    lineCount++;
//                    string line = reader.ReadLine();
//                    string[] values = GetLineValues(line, settings.Delimiter);
//                    if (lineCount == 1) // Headers
//                    {
//                        headers = values;
//                        for (int index = 0; index < headers.Length; index++)
//                        {
//                            if (columnTypes == null || columnTypes.Count == 0)
//                            {
//                                dataTable.Columns.Add(headers[index]);
//                            }
//                            else
//                            {
//                                dataTable.Columns.Add(headers[index], columnTypes[index]);
//                            }                       
//                        }
//                    }
//                    else
//                    {
//                        DataRow row = dataTable.NewRow();
//                        int columnCount = 0;
//                        for (int index = 0; index < values.Length; index++)
//                        {
//                            columnCount++;
//                            if (columnTypes == null || columnTypes.Count == 0)
//                            {
//                                row[columnCount - 1] = DeserializeValue(values[index], typeof(String)); 
//                            }
//                            else
//                            {
//                                row[columnCount - 1] = DeserializeValue(values[index], columnTypes[index]);
//                            }

//                        }
//                        dataTable.Rows.Add(row);
//                    }
//                }
//                reader.Close();
//            }
//            return dataTable;            
//        }

//        private object DeserializeValue(string value, Type type)
//        {
//            return Convert.ChangeType(value, type);
//        }

//        private string[] GetLineValues(string line, Char delimimiter)
//        {
//            string[] values = line.Split(delimimiter);
//            return values;
//        }
//    }
//}
