using CFCSV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.Reader
{
    /// <summary>
    /// Reads CSV and returns Dictionary per row, key is column name
    /// </summary>
    public class CSVDictionaryReader : CSVReaderBase
    {
        private interface IPropertyMapping
        {
            /// <summary>
            /// Sets property value for entity
            /// </summary>
            /// <param name="entity"></param>
            /// <param name="values"></param>
            void SetValue(Dictionary<string, object> entity, List<string> headers, List<string> values);
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
            public string DictionaryKey { get; internal set; }

            public PropertyMapping(Expression<Func<List<string>, List<string>, TMySourceType>> valuesProperty,
                                   string dictionaryKey)
            {
                ValuesProperty = valuesProperty;
                DictionaryKey = dictionaryKey;
            }

            public void SetValue(Dictionary<string, object> entity, List<string> headers, List<string> values)
            {
                // Get property value from column value(s)
                var valuesFunction = ValuesProperty.Compile();
                var propertyValue = valuesFunction(headers, values);

                // Set entity property                
                entity[DictionaryKey] = propertyValue;                
            }
        }

        private List<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public string File { get; set; } = string.Empty;
        public char Delimiter { get; set; } = (char)9;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        
        /// <summary>
        /// Reads CSV rows and returns Dictionary[string, object] for each row.
        /// </summary>
        /// <param name="createEntity">Function to create default Dictionary</param>
        /// <param name="maxRows">Max rows to return (null=No limit)</param>
        /// <returns></returns>
        public IEnumerable<Dictionary<string, object>> Read(Expression<Func<Dictionary<string, object>>> createEntity,
                                                          int? maxRows = null)
        {
            //var items = new List<Dictionary<string, object>>();

            using (StreamReader reader = new StreamReader(File))
            {
                int lineCount = 0;
                var headers = new List<string>();
                const Char quotes = '"';

                var createEntityFunction = createEntity.Compile();

                var gotAllRequiredRows = false;
                while (!reader.EndOfStream &&
                    !gotAllRequiredRows)
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

                        yield return entity;

                        // Abort if row limit reached
                        if (maxRows != null && lineCount - 1 >= maxRows) gotAllRequiredRows = true;                        
                    }
                }
            }            
        }

        /// <summary>
        /// Adds mapping between entity property and CSV values
        /// </summary>
        /// <typeparam name="TSourceType"></typeparam>
        /// <param name="dictionaryKey">Dictionary key</param>
        /// <param name="valuesProperty">Expression to return dictionary value from CSV headers and row values</param>
        public void AddPropertyMapping<TSourceType>(string dictionaryKey,
                                            Expression<Func<List<string>, List<string>, TSourceType>> valuesProperty)
        {
            _propertyMappings.Add(new PropertyMapping<TSourceType>(valuesProperty, dictionaryKey));
        }
    }
}
