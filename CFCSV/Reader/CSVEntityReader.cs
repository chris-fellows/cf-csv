using CFCSV.Models;
using CFCSV.Utilities;

namespace CFCSV.Reader
{
    /// <summary>
    /// Reads CSV to list of type T
    /// </summary>
    public class CSVEntityReader<TEntity>
    {
        /// <summary>
        /// Reads CSV and returns a list of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvSettings"></param>
        /// <param name="functionGetObjectFromColumnValues"></param>
        /// <returns></returns>
        public List<TEntity> Read(CSVSettings csvSettings,
                            Func<List<object>, TEntity> functionGetObjectFromColumnValues)
        {                                 
            List<TEntity> items = new List<TEntity>();

            using (StreamReader reader = new StreamReader(csvSettings.Filename))
            {
                int lineCount = 0;
                while (!reader.EndOfStream)
                {
                    lineCount++;
                    
                    // Read line
                    string line = reader.ReadLine();

                    if (lineCount > 1)
                    {
                        // Get column values
                        List<object> columnValues = CSVUtilities.GetColumnValues(line, csvSettings);

                        // Create object from column values
                        TEntity item = functionGetObjectFromColumnValues(columnValues);

                        items.Add(item);
                    }
                }
            }

            return items;
        }              
     }
}
