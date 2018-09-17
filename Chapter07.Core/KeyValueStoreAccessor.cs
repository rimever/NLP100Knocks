using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace Chapter07.Core
{
    /// <summary>
    /// KeyValueStoreデータへのアクセスを行うためのクラスです。
    /// </summary>
    public class KeyValueStoreAccessor
    {
        private readonly string _connectionString;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyValueStoreAccessor()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Chapter07.Core\connectionString.txt");
            _connectionString = File.ReadAllText(path);
        }
        /// <summary>
        /// アーティスト名から場所を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAreaByArtistName(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select kvs -> 'area'  as area from artist where kvs -> 'name' = :name";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));
                command.Parameters["name"].Value = name;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    yield return (dataReader["area"] ?? "(不明)").ToString();
                }
            }
        }
        /// <summary>
        /// 場所からアーティストを取得します。
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IEnumerable<string> GetArtistNameByArea(string area)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "select kvs -> 'name'  as name from artist where kvs -> 'area' = :area";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("area", NpgsqlDbType.Varchar));
                command.Parameters["area"].Value = area;
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    yield return dataReader["name"].ToString();
                }
            }
        }
    }
}
