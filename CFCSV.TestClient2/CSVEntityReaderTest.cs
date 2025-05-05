using CFCSV.Models;
using CFCSV.Reader;
using CFCSV.TestClient.Models;
using CFCSV.Utilities;
using System.Text;

namespace CFCSV.TestClient
{
    /// <summary>
    /// CSV entity reader test
    /// </summary>
    internal class CSVEntityReaderTest
    {
        public void ReadCustomObjects(string file)
        {           
            Char quotes = '"';
          
            var csvReader = new CSVEntityReader<TestObject1>()
            {
                File = file,
                Delimiter = (Char)9,
                Encoding = Encoding.UTF8
            };

            // Set property mappings
            var nullString = "null";
            csvReader.AddPropertyMapping<String>(e => e.Id, (headers, values) => values[0]);
            csvReader.AddPropertyMapping<DateTimeOffset>(e => e.DateTimeOffsetValue, (headers, values) => DateTimeOffset.Parse(CSVUtilities.RemoveOuterQuotes(values[1], quotes)));
            csvReader.AddPropertyMapping<Int16>(e => e.Int16Value, (headers, values) => Convert.ToInt16(values[2]));
            csvReader.AddPropertyMapping<Int32>(e => e.Int32Value, (headers, values) => Convert.ToInt32(values[3]));
            csvReader.AddPropertyMapping<Int32?>(e => e.Inv32ValueNullable, (headers, values) => values[4].Equals(nullString) ? null : Convert.ToInt32(values[4]));
            csvReader.AddPropertyMapping<Int64>(e => e.Int64Value, (headers, values) => Convert.ToInt64(values[5]));
            csvReader.AddPropertyMapping<Boolean>(e => e.BooleanValue, (headers, values) => Convert.ToBoolean(values[6]));
            csvReader.AddPropertyMapping<String>(e => e.StringValue, (headers, values) => CSVUtilities.RemoveOuterQuotes(values[7], quotes));
            // null value written without quotes (Has quotes when loaded), "null" literal will have another set of quotes
            csvReader.AddPropertyMapping<String?>(e => e.StringValueNullable, (headers, values) => values[8].Equals(nullString) ? null : CSVUtilities.RemoveOuterQuotes(values[8], quotes));

            // Read objects
            //var testObjects = csvReader.Read(() => new TestObject1());
            foreach(var testObject in csvReader.Read(() => new TestObject1()))
            {
                int zzzz = 10000;
            }

            int xxxxxx = 1000;
        }      
    }
}
