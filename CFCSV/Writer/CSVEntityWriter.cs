﻿using CFCSV.Utilities;
using System.Linq.Expressions;
using System.Text;

namespace CFCSV.Writer
{
    /// <summary>
    /// CSV writer for list entities of specific type
    /// </summary>
    /// <typeparam name="TEntity">Entity to write</typeparam>
    public class CSVEntityWriter<TEntity>
    {
        private interface IColumnMapping
        {
            string Name { get; }

            string GetValue<T>(T entity);
        }

        private class ColumnMapping<TMySourceType> : IColumnMapping
        {           
            public string Name { get; internal set; }

            public Expression<Func<TEntity, TMySourceType>> SourceProperty { get; internal set; }

            public Expression<Func<TMySourceType, string>> DestinationProperty { get; internal set; }        

            public ColumnMapping(string name,
                           Expression<Func<TEntity, TMySourceType>> sourceProperty,
                           Expression<Func<TMySourceType, string>> destinationProperty)
            {
                Name = name;
                SourceProperty = sourceProperty;
                DestinationProperty = destinationProperty;
            }

            public string GetValue<T>(T entity)
            {
                var propertyInfo = SourceProperty.GetPropertyInfo();

                var propertyValue = (TMySourceType)propertyInfo.GetValue(entity);

                var myFunction = DestinationProperty.Compile();
                return myFunction(propertyValue);                
            }
        }
        
        private List<IColumnMapping> _columnMappings = new List<IColumnMapping>();

        public string File { get; set; } = string.Empty;
        public char Delimiter { get; set; } = (char)9;
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Adds column
        /// </summary>
        /// <typeparam name="TSourceType"></typeparam>
        /// <param name="sourceProperty"></param>
        /// <param name="destinationProperty"></param>
        public void AddColumn<TSourceType>(string name,
                                           Expression<Func<TEntity, TSourceType>> sourceProperty,
                                           Expression<Func<TSourceType, string>> destinationProperty)

        {
            var columnMapping = new ColumnMapping<TSourceType>(name, sourceProperty, destinationProperty);

            _columnMappings.Add(columnMapping);
        }

        public void Write(IEnumerable<TEntity> entities)
        {
            // Delete exising file
            if (System.IO.File.Exists(File))
            {
                System.IO.File.Delete(File);
            }

            using (var streamWriter = new StreamWriter(File, true, Encoding))
            {
                // Write header
                var headers = new StringBuilder("");
                foreach (IColumnMapping columnMapping in _columnMappings)
                {
                    if (headers.Length > 0) headers.Append(Delimiter);
                    headers.Append(columnMapping.Name);
                }
                streamWriter.WriteLine(headers.ToString());

                // Write entities
                foreach (var entity in entities)
                {
                    var line = GetLineValues(entity);
                    streamWriter.WriteLine(line.ToString());
                }
            }
        }

        private string GetLineValues(TEntity entity)
        {
            var line = new StringBuilder("");

            foreach (IColumnMapping columnMapping in _columnMappings)
            {
                if (line.Length > 0) line.Append(Delimiter);
                var value = columnMapping.GetValue(entity);
                line.Append(value);
            }

            return line.ToString();
        }
    }
}
