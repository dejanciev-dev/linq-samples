using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LinqSamples.Common.Extensions
{
    public static class ListExtensions
    {
        public static string ConvertToString<T>(this List<T> objects)
        {
            var sb = new StringBuilder();
            foreach (var obj in objects)
            {
                var type = obj.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
                var propSb = new StringBuilder();
                foreach (var prop in props)
                {
                    object propValue = prop.GetValue(obj, null);

                    propSb.Append(propValue.ToString() + " ");
                }
                propSb.Remove(propSb.Length - 1, 1);
                sb.AppendLine(propSb.ToString());
            }
            return sb.ToString();
        }
    }
}