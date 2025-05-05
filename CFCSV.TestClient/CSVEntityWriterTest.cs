using CFCSV.TestClient.Models;
using CFCSV.Writer;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void WriteCustomObjects(string file)
        {
            var random = new Random();

            if (File.Exists(file))
            {
                File.Delete(file);
            }

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
                    StringValue = "Test Value",
                    StringValueNullable = null
                    //StringValueNullable = "null"
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
            var nullString = "null";
            csvWriter.AddColumn<string>("Id", u => u.Id, value => value.ToString());
            csvWriter.AddColumn<DateTimeOffset>("DateTimeOffsetValue", u => u.DateTimeOffsetValue, value => $"{quotes}{value.ToString()}{quotes}");
            csvWriter.AddColumn<Int16>("Int16Value", u => u.Int16Value, value => value.ToString());
            csvWriter.AddColumn<Int32>("Int32Value", u => u.Int32Value, value => value.ToString());
            csvWriter.AddColumn<Int32?>("Int32ValueNullable", u => u.Inv32ValueNullable, value => value == null ? nullString : value.ToString());
            csvWriter.AddColumn<Int64>("Int64Value", u => u.Int64Value, value => value.ToString());
            csvWriter.AddColumn<Boolean>("BooleanValue", u => u.BooleanValue, value => value.ToString());
            csvWriter.AddColumn<String>("StringValue", u => u.StringValue, value => $"{quotes}{value.ToString()}{quotes}");
            csvWriter.AddColumn<String?>("StringValueNullable", u => u.StringValueNullable, value => value == null ? nullString : $"{quotes}{value.ToString()}{quotes}");

            // Write to CSV
            csvWriter.Write(testObjects);
        }

        public void WriteDictionaryObjects(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var csvWriter = new CSVEntityWriter<Dictionary<string, object>>()
            {
                Delimiter = (Char)9,
                Encoding = Encoding.UTF8,
                File = file
            };

            // Set column mappings
            var quotes = '"';
            var nullString = "null";
            csvWriter.AddColumn<object>("Id", u => u["Id"], value => value.ToString());
            csvWriter.AddColumn<object>("DateTimeOffsetValue", u => u["DateTimeOffsetValue"], value => $"{quotes}{value.ToString()}{quotes}");
            csvWriter.AddColumn<object>("Int32Value", u => u["Int32Value"], value => value.ToString());
            csvWriter.AddColumn<object>("Int32ValueNullable", u => u["Inv32ValueNullable"], value => value == null ? nullString : value.ToString());
            csvWriter.AddColumn<object>("BooleanValue", u => u["BooleanValue"], value => value.ToString());

            for (int index =0; index < 100; index++)
            {
                DateTimeOffset dateTimeOffsetValue = DateTimeOffset.UtcNow;
                Int32 int32Value = 1000;
                Int32? int32ValueNullable = null;
                bool booleanValue = true;

                var row = new Dictionary<string, object>()
                {
                    { "Id", Guid.NewGuid().ToString() },
                    { "DateTimeOffsetValue", dateTimeOffsetValue },
                    { "Int32Value", int32Value },
                    { "Int32ValueNullable", int32ValueNullable },
                    { "BooleanValue", booleanValue }
                };

                // Write to CSV
                csvWriter.Write(new[] { row });
            }
        }

        public void WriteObjectArrayObjects(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var csvWriter = new CSVEntityWriter<object[]>()
            {
                Delimiter = (Char)9,
                Encoding = Encoding.UTF8,
                File = file
            };

            // Set column mappings
            var quotes = '"';
            var nullString = "null";
            csvWriter.AddColumn<object>("Id", u => u[0], value => value.ToString());
            csvWriter.AddColumn<object>("DateTimeOffsetValue", u => u[1], value => $"{quotes}{value.ToString()}{quotes}");
            csvWriter.AddColumn<object>("Int32Value", u => u[2], value => value.ToString());
            csvWriter.AddColumn<object>("Int32ValueNullable", u => u[3], value => value == null ? nullString : value.ToString());
            csvWriter.AddColumn<object>("BooleanValue", u => u[4], value => value.ToString());

            for (int index = 0; index < 100; index++)
            {
                DateTimeOffset dateTimeOffsetValue = DateTimeOffset.UtcNow;
                Int32 int32Value = 1000;
                Int32? int32ValueNullable = null;
                bool booleanValue = true;

                var newObject = new object[] 
                {
                    Guid.NewGuid().ToString(),
                    dateTimeOffsetValue,
                    int32Value,
                    int32ValueNullable,
                    booleanValue
                };

                // Write to CSV
                csvWriter.Write(new[] { newObject });
            }
        }   
    }
}
