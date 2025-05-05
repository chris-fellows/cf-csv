using CFCSV.Reader;
using CFCSV.TestClient.Models;
using CFCSV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.TestClient2
{
    internal class CSVDictionaryReaderTest
    {
        public void ReadDictionaryObjects(string file)
        {
            Char quotes = '"';

            var csvReader = new CSVDictionaryReader()
            {
                File = file,
                Delimiter = (Char)9,
                Encoding = Encoding.UTF8
            };

            // Set property mappings
            var nullString = "null";
            csvReader.AddPropertyMapping<String>("RowId", 
                    (headers, values) => values[headers.IndexOf("Id")]);
            csvReader.AddPropertyMapping<DateTimeOffset>("RowDateTimeOffsetValue", 
                    (headers, values) => DateTimeOffset.Parse(CSVUtilities.RemoveOuterQuotes(values[headers.IndexOf("DateTimeOffsetValue")], quotes)));
            csvReader.AddPropertyMapping<Int32?>("RowInv32ValueNullable", 
                    (headers, values) => values[headers.IndexOf("Int32ValueNullable")].Equals(nullString) ? null : Convert.ToInt32(values[headers.IndexOf("Int32ValueNullable")]));

            // Read objects
            foreach (var row in csvReader.Read(() => new Dictionary<string, object>()
                {
                    { "RowId", null },
                    { "RowDateTimeOffsetValue", null },
                    { "RowInv32ValueNullable", null }
                }))
            {
                var id = row["RowId"];
                int xxx = 1000;
            }
                                          
            int xxxxxx = 1000;
        }
    }
}
