namespace CFCSV.Models
{
    /// <summary>
    /// CSV column
    /// </summary>
    public class CSVColumn
    {
        public string Name { get; set; } = String.Empty;

        public Type DataType { get; set; }

        public bool HeaderQuoted { get; set; }
        
        public bool ValueQuoted { get; set; }

        public string FormatString { get; set; } = String.Empty;

        public string NullValueString { get; set; } = String.Empty;
    }
}
