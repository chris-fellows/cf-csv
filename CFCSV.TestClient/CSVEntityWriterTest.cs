using CFCSV.TestClient.Models;
using CFCSV.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.TestClient
{
    /// <summary>
    /// CSV entity writer test
    /// </summary>
    internal class CSVEntityWriterTest
    {
        public void Run(string file)
        {
            var random = new Random();

            var testObjects = new List<TestObject1>();

            for (int index = 0; index < 100; index++)
            {
                var testObject = new TestObject1()
                {
                    Id = Guid.NewGuid().ToString(),
                    BooleanValue = true,
                    DateTimeOffsetValue = DateTimeOffset.UtcNow,
                    Int16Value = 1020,
                    Int32Value = 1021,
                    Inv32ValueNullable = null,
                    Int64Value = 1022,
                    StringValue = "Test value"                    
                };
                testObjects.Add(testObject);
            }

            var csvWriter = new CSVEntityWriter<TestObject1>()
            {
                Delimiter = (Char)9,
                Encoding = Encoding.UTF8,
                File = file
            };

            // Set column mappings
            var quotes = '"';
            csvWriter.AddColumn<string>("Id", u => u.Id, value => value.ToString());
            csvWriter.AddColumn<DateTimeOffset>("DateTimeOffsetValue", u => u.DateTimeOffsetValue, value => $"{quotes}{value.ToString()}{quotes}");
            csvWriter.AddColumn<Int16>("Int16Value", u => u.Int16Value, value => value.ToString());
            csvWriter.AddColumn<Int32>("Int32Value", u => u.Int32Value, value => value.ToString());
            csvWriter.AddColumn<Int32?>("Int32ValueNullable", u => u.Inv32ValueNullable, value => value.HasValue ? value.ToString() : "");
            csvWriter.AddColumn<Int64>("Int64Value", u => u.Int64Value, value => value.ToString());
            csvWriter.AddColumn<Boolean>("BooleanValue", u => u.BooleanValue, value => value.ToString());
            csvWriter.AddColumn<String>("StringValue", u => u.StringValue, value => $"{quotes}{value}{quotes}");

            csvWriter.Write(testObjects);
        }
    }
}
