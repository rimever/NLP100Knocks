using System.Collections.Generic;
using Chapter07.Core;
using FluentMigrator;
using Newtonsoft.Json.Linq;

namespace Chapter07.Migrator
{
    [Migration(2)]
    public class InsertTableMigration : Migration
    {
        public override void Up()
        {
            var reader = new DataSourceJsonReader();
            foreach (var line in reader.EnumerableLine())
            {
                var jsonLine = line.Replace("'", @"''");
                string sql = $@"INSERT INTO artist (kvs,json) VALUES ('{ConvertHstoreValue(line)}' , '{jsonLine}')";
                Execute.Sql(sql);
            }
        }

        /// <summary>
        /// hstoreの値に変換します。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ConvertHstoreValue(string line)
        {
            var json = JObject.Parse(line);
            var dictionary = json.ToObject<Dictionary<string, object>>();
            List<string> array = new List<string>();
            foreach (var keyPair in dictionary)
            {
                var value = keyPair.Value.ToString()
                    .Replace(@"\", @"\\")
                    .Replace(@"""", @"\""")
                    .Replace("'", @"''");
                array.Add($@"""{keyPair.Key}"" => ""{value}""");
            }

            string result = $"{string.Join(",", array.ToArray())}";
            return result;
        }


        public override void Down()
        {
            Delete.Table("artist");
        }
    }
}