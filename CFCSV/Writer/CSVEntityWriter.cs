﻿using CFCSV.Utilities;
using System.Linq.Expressions;
using System.Text;

namespace CFCSV.Writer
{
    /// <summary>
    /// CSV writer for custom objects
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CSVEntityWriter<TEntity>
    {
        private interface IColumnMapping
        {
            string Name { get; }

            string GetValue(TEntity entity);
        }

        /// <summary>
        /// Mapping between CSV column and entity property
        /// </summary>
        /// <typeparam name="TMySourceType"></typeparam>
        private class ColumnMapping : IColumnMapping
        {
            public string Name { get; internal set; }

            //public Expression<Func<TEntity, TMySourceType>> SourceProperty { get; internal set; }

            public Expression<Func<TEntity, string>> DestinationProperty { get; internal set; }

            public ColumnMapping(string name,                           
                           Expression<Func<TEntity, string>> destinationProperty)
            {
                Name = name;                
                DestinationProperty = destinationProperty;
            }

            public string GetValue(TEntity entity)
            {
                var destinationPropertyCompiled = DestinationProperty.Compile();
                var result = destinationPropertyCompiled(entity);
                return result;

                /*
                var propertyInfo = SourceProperty.GetPropertyInfo();

                var propertyValue = (TMySourceType)propertyInfo.GetValue(entity);

                var myFunction = DestinationProperty.Compile();
                return myFunction(propertyValue);
                */
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
        public void AddColumn(string name,                                          
                              Expression<Func<TEntity, string>> destinationProperty)

        {
            _columnMappings.Add(new ColumnMapping(name, destinationProperty));
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
        public void Write(IEnumerable<TEntity> entities)
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
