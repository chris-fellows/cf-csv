using CFCSV.Models;
using CFCSV.Reader;
using CFCSV.TestClient.Models;
using System.Text;

namespace CFCSV.TestClient
{
    /// <summary>
    /// CSV entity reader test
    /// </summary>
    internal class CSVEntityReaderTest
    {
        public void Run(string file)
        {
            var csvSettings = new CSVSettings()
            {
                 Delimiter = (Char)9,
                 Encoding = Encoding.UTF8,
                 Filename = file,
                 Columns = new List<CSVColumn>()
                 {
                     new CSVColumn()
                     {
                          Name = "Id",
                          DataType = typeof(String),
                          ValueQuoted = false
                     },
                     new CSVColumn()
                     {
                          Name = "DateTimeOffsetValue",
                          DataType = typeof(DateTimeOffset),
                          ValueQuoted = true
                     },
                     new CSVColumn()
                     {
                          Name = "Int16Value",
                          DataType = typeof(Int16),
                          ValueQuoted = false
                     },
                     new CSVColumn()
                     {
                          Name = "Int32Value",
                          DataType = typeof(Int32),
                          ValueQuoted = false
                     },
                     new CSVColumn()
                     {
                          Name = "Int32ValueNullable",
                          DataType = typeof(Int32?),
                          ValueQuoted = false
                     },
                     new CSVColumn()
                     {
                          Name = "Int64Value",
                          DataType = typeof(Int64),
                          ValueQuoted = false
                     },
                       new CSVColumn()
                     {
                          Name = "BooleanValue",
                          DataType = typeof(Boolean),
                          ValueQuoted = false
                     },
                         new CSVColumn()
                     {
                          Name = "StringValue",
                          DataType = typeof(String),
                          ValueQuoted = true
                     },
                 }
            };

            // Read CSV
            var csvReader = new CSVEntityReader<TestObject1>();
            var testObjects = csvReader.Read(csvSettings, (values) =>
            {                
                return new TestObject1()
                {
                    Id = (string)values[0],
                    DateTimeOffsetValue  = (DateTimeOffset)values[1],
                    Int16Value = (Int16)values[2],
                    Int32Value = (Int32)values[3],
                    Inv32ValueNullable = (Int32?)values[4],
                    Int64Value = (Int64)values[5],
                    BooleanValue = (Boolean)values[6],
                    StringValue = (String)values[7],
                };
            });

            int xxxxxx = 1000;
        }
    }
}
