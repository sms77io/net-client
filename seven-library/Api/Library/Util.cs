using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace seven_library.Api.Library
{
    public class Util
    {
        public static string[] SplitByLine(string str)
        {
            return str.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string LcFirst(string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static string ToString(JToken? token)
        {
            if (token.Type == JTokenType.String)
            {
                return token.ToObject<string>();
            }

            return token.Type == JTokenType.Boolean ? $"{Convert.ToInt32(token.ToObject<bool>())}" : token.ToString();
        }

        public static IDictionary<string, object> ToDictionary(object @params, string exclude = null)
        {
            var dict = (IDictionary<string, object>) new ExpandoObject();

            foreach (var property in @params.GetType().GetProperties())
            {
                if (exclude == property.Name) {
                    continue;
                }
                
                var value = property.GetValue(@params);

                if (null != value)
                {
                    dict.Add(property.Name, value);
                }
            }

            return dict;
        }

        public static bool IsValidDate(string date, string format)
        {
            return DateTime.TryParseExact(
                date,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }

        public static string ToTitleCase(string str)
        {
            return str.Substring(0, 1) + str.Substring(1).ToLower();
        }

        public static JObject? ToJObject(object paras)
        {
            var json = JsonConvert.SerializeObject(
                paras,
                Formatting.None,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            );
            
            Console.WriteLine(json);

            return JsonConvert.DeserializeObject<JObject>(json);
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}