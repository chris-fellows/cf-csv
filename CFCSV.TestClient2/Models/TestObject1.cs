namespace CFCSV.TestClient.Models
{
    internal class TestObject1
    {
        public string Id { get; set; } = String.Empty;

        public DateTimeOffset DateTimeOffsetValue { get; set; }

        public bool BooleanValue { get; set; }

        public Int16 Int16Value { get; set; }

        public Int32 Int32Value { get; set; }

        public Int32? Inv32ValueNullable { get; set; }

        public Int64 Int64Value { get; set; }        

        public string StringValue { get; set; } = String.Empty;

        public string? StringValueNullable { get; set; }
    }
}
