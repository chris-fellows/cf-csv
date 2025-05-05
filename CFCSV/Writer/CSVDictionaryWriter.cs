using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.Writer
{
    /// <summary>
    /// CSV for dictionary objects. Each row is a Dictionary<string, object>
    /// </summary>
    public class CSVDictionaryWriter
    {
        private interface IColumnMapping
        {
            string Name { get; }

            string GetValue<T>(Dictionary<string, object> entity);
        }

        /// <summary>
        /// Mapping between CSV column and entity property
        /// </summary>
        /// <typeparam name="TMySourceType"></typeparam>
        private class ColumnMapping<TMySourceType> : IColumnMapping
        {
            public string Name { get; internal set; }
            
            public Expression<Func<Dictionary<string, object>, string>> DestinationProperty { get; internal set; }

            public ColumnMapping(string name,                           
                           Expression<Func<Dictionary<String, object>, string>> destinationProperty)
            {
                Name = name;                
                DestinationProperty = destinationProperty;
            }

            public string GetValue<T>(Dictionary<string, object> entity)
            {                                
                var destinationPropertyCompiled = DestinationProperty.Compile();
                var result = destinationPropertyCompiled(entity);
                return result;                
            }
        }

        private List<IColumnMapping> _columnMappings = new List<IColumnMapping>();

        public string File { get; set; } = string.Empty;
        public char Delimiter { get; set; } = (char)9;
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public void AddColumn(string name,                                      
                                      Expression<Func<Dictionary<string, object>, string>> destinationProperty)

        {
            _columnMappings.Add(new ColumnMapping<string>(name, destinationProperty));
        }

        public void Delete()
        {
            if (System.IO.File.Exists(File))
            {
                System.IO.File.Delete(File);
            }
        }

        /// <summary>
        /// Writes rows
        /// </summary>
        /// <param name="entities"></param>
        public void Write(IEnumerable<Dictionary<string, object>> entities)
        {
            /*
            // Delete exising file
            if (System.IO.File.Exists(File))
            {
                System.IO.File.Delete(File);
            }
            */

            var isWriteHeader = !System.IO.File.Exists(File);

            using (var streamWriter = new StreamWriter(File, true, Encoding))
            {
                // Write header
                if (isWriteHeader)
                {
                    var headers = new StringBuilder("");
                    foreach (IColumnMapping columnMapping in _columnMappings)
                    {
                        if (headers.Length > 0) headers.Append(Delimiter);
                        headers.Append(columnMapping.Name);
                    }
                    streamWriter.WriteLine(headers.ToString());
                }

                // Write entities
                foreach (var entity in entities)
                {
                    var line = GetLineValues(entity);
                    streamWriter.WriteLine(line.ToString());
                }
            }
        }

        //public void Write(TEntity entity)
        //{
        //    // TODO: Make this more efficient so that we don't open & close file each time
        //    Write(new[] { entity });   
        //}

        private string GetLineValues(Dictionary<string, object> entity)
        {
            var line = new StringBuilder("");

            foreach (IColumnMapping columnMapping in _columnMappings)
            {
                if (line.Length > 0) line.Append(Delimiter);
                var value = columnMapping.GetValue<Dictionary<string, object>>(entity);
                line.Append(value);
            }

            return line.ToString();
        }
    }
}
