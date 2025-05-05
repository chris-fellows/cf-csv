using CFCSV.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.TestClient2
{
    internal class CSVDictionaryWriterTest
    {
        public void WriteDictionaryObjects(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var csvWriter = new CSVDictionaryWriter()
            {
                Delimiter = (Char)9,
                Encoding = Encoding.UTF8,
                File = file
            };

            // Set column mappings
            var quotes = '"';
            var nullString = "null";
            csvWriter.AddColumn("Id", row => row["RowId"].ToString());
            csvWriter.AddColumn("DateTimeOffsetValue", row => $"{quotes}{row["RowDateTimeOffsetValue"].ToString()}{quotes}");
            csvWriter.AddColumn("Int32Value", row => row["RowInt32Value"].ToString());
            csvWriter.AddColumn("Int32ValueNullable", row => row["RowInt32ValueNullable"] == null ? nullString : row["RowInt32ValueNullable"].ToString());
            csvWriter.AddColumn("BooleanValue", row => row["RowBooleanValue"].ToString());

            for (int index = 0; index < 100; index++)
            {
                DateTimeOffset dateTimeOffsetValue = DateTimeOffset.UtcNow;
                Int32 int32Value = 1000;
                Int32? int32ValueNullable = null;
                bool booleanValue = true;

                var row = new Dictionary<string, object>()
                {
                    { "RowId", Guid.NewGuid().ToString() },
                    { "RowDateTimeOffsetValue", dateTimeOffsetValue },
                    { "RowInt32Value", int32Value },
                    { "RowInt32ValueNullable", int32ValueNullable },
                    { "RowBooleanValue", booleanValue }
                };

                // Write to CSV
                csvWriter.Write(new[] { row });
            }
        }
    }
}
