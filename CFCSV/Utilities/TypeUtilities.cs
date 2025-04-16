using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCSV.Utilities
{
    internal class TypeUtilities        
    {
        /// <summary>
        /// Returns whether all values can be converted to the type
        /// </summary>
        /// <param name="values"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CanConvertAllValuesToType(List<object> values, Type type)
        {
            for (int index = 0; index < values.Count; index++)
            {
                bool canConvert = false;

                try
                {
                    object convertedValue = ConvertValueToType(values[index], type);
                    canConvert = true;
                }
                catch { }
                ;

                if (!canConvert)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Converts the value to the specified type. Convert.ChangeType does not work with all data types.
        /// 
        /// When using NullableConverter then, for some types (E.g. Single) then it's necessary to convert the value passed to
        /// ConvertFrom otherwise there are exceptions. E.g. "SingleConverter cannot convert from System.Double"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertValueToType(object value, Type type)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(String) && type == typeof(DateTimeOffset))
                {
                    return DateTimeOffset.Parse(value.ToString());             
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    // Nullable type, user NullableConvert to convert
                    NullableConverter nullableConverter = new NullableConverter(type);
                    if (type == typeof(Nullable<System.Single>))
                    {
                        value = nullableConverter.ConvertFrom(Convert.ToSingle(value));
                    }
                    else if (type == typeof(Nullable<System.Double>))
                    {
                        value = nullableConverter.ConvertFrom(Convert.ToDouble(value));
                    }
                    else
                    {
                        value = nullableConverter.ConvertFrom(value);
                    }
                }
                else if (type.IsEnum)
                {
                    // Enum
                    value = Enum.ToObject(type, value);
                }
                else if (type == typeof(Object))
                {
                    // No action needed, calling ChangeType below may cause error too                    
                }
                else
                {
                    value = Convert.ChangeType(value, type);
                }
            }
            return value;
        }
    }
}
