using CFCSV.Models;
using CFCSV.Utilities;
using System.Linq.Expressions;
using System.Text;

namespace CFCSV.Reader
{
    /// <summary>
    /// Reads CSV to list of type T
    /// </summary>
    public class CSVEntityReader<TEntity>
    {
        private interface IPropertyMapping
        {            
            /// <summary>
            /// Sets property value for entity
            /// </summary>
            /// <param name="entity"></param>
            /// <param name="values"></param>
            void SetValue(TEntity entity, List<string> headers, List<string> values);
        }

        /// <summary>
        /// Mapping between entity property and CSV value(s). Typically the entity property comes from one CSV column but
        /// theoretically it could come from multiple properties.
        /// </summary>
        /// <typeparam name="TMySourceType"></typeparam>
        private class PropertyMapping<TMySourceType> : IPropertyMapping
        {            
            /// <summary>
            /// Expression to convert CSV value(s) to entity property value
            /// </summary>
            public Expression<Func<List<string>, List<string>, TMySourceType>> ValuesProperty { get; internal set; }

            /// <summary>
            /// Expression to return entity property to set
            /// </summary>
            public Expression<Func<TEntity, TMySourceType>> EntityProperty { get; internal set; }

            public PropertyMapping(Expression<Func<List<string>, List<string>, TMySourceType>> valuesProperty,
                           Expression<Func<TEntity, TMySourceType>> entityProperty)
            {                
                ValuesProperty = valuesProperty;
                EntityProperty = entityProperty;
            }

            public void SetValue(TEntity entity, List<string> headers, List<string> values)
            {
                // Get property value from column value(s)
                var valuesFunction = ValuesProperty.Compile();
                var propertyValue = valuesFunction(headers, values);

                // Set entity property
                var entityPropertyInfo = EntityProperty.GetPropertyInfo();
                entityPropertyInfo.SetValue(entity, (TMySourceType)propertyValue);
            }
        }

        private List<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public string File { get; set; } = string.Empty;
        public char Delimiter { get; set; } = (char)9;
        public Encoding Encoding { get; set; } = Encoding.UTF8;       

        public List<TEntity> Read(Expression<Func<TEntity>> createEntity)
        {
            List<TEntity> items = new List<TEntity>();

            using (StreamReader reader = new StreamReader(File))
            {
                int lineCount = 0;
                var headers = new List<string>();
                const Char quotes = '"';

                var createEntityFunction = createEntity.Compile();

                while (!reader.EndOfStream)
                {
                    lineCount++;

                    // Read line
                    string line = reader.ReadLine();

                    if (lineCount == 1)   // Header
                    {
                        headers = CSVUtilities.GetColumnValueStrings(line, Delimiter, quotes);
                    }
                    else     // Row
                    {                        
                        // Get CSV column values
                        var values = CSVUtilities.GetColumnValueStrings(line, Delimiter, quotes);

                        // Set entity properties
                        var entity = createEntityFunction.Invoke();
                        foreach (IPropertyMapping propertyMapping in _propertyMappings)
                        {
                            propertyMapping.SetValue(entity, headers, values);
                        }

                        items.Add(entity);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Adds mapping between entity property and CSV values
        /// </summary>
        /// <typeparam name="TSourceType"></typeparam>
        /// <param name="entityProperty"></param>
        /// <param name="valuesProperty"></param>
        public void AddPropertyMapping<TSourceType>(Expression<Func<TEntity, TSourceType>> entityProperty,
                                                Expression<Func<List<string>, List<string>, TSourceType>> valuesProperty)                                           
        {            
            _propertyMappings.Add(new PropertyMapping<TSourceType>(valuesProperty, entityProperty));
        }
    }
}
