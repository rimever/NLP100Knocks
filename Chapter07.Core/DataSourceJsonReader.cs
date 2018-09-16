using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Chapter07.Core
{
    public class DataSourceJsonReader
    {
        private readonly string _textFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter07.Core\artist.json");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataSourceJsonReader()
        {
            Debug.Assert(File.Exists(_textFilePath));
        }
        /// <summary>
        /// json情報を列挙します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> EnumerableLine()
        {
            using (var reader = new StreamReader(_textFilePath, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    yield return line;
                }
            }
        }
        /// <summary>
        /// json情報を列挙します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<JObject> EnumerableJson()
        {
            using (var reader = new StreamReader(_textFilePath, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    yield return JObject.Parse(line);
                }
            }
        }

        public IEnumerable<IDictionary<string, object>> EnumerableDictionary()
        {
            using (var reader = new StreamReader(_textFilePath, Encoding.UTF8))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    var jobj = JObject.Parse(line);
                    yield return jobj.ToObject<Dictionary<string, object>>();
                }
            }

        }
    }
}
