#region

using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;

#endregion

namespace Chapter07.Core
{
    /// <summary>
    /// KeyValueStoreデータへのアクセスを行うためのクラスです。
    /// </summary>
    public class KeyValueStoreAccessor
    {
        private readonly ConnectionString _connectionString = new ConnectionString();

        /// <summary>
        /// アーティスト名から場所を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<string> GetAreaByArtistName(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = "select kvs -> 'area'  as area from artist where kvs -> 'name' = :name";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));
                command.Parameters["name"].Value = name;
                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add((dataReader["area"] ?? "(不明)").ToString());
                }

                return results;
            }
        }

        /// <summary>
        /// 場所からアーティストを取得します。
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IEnumerable<string> GetArtistNameByArea(string area)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = "select kvs -> 'name'  as name from artist where kvs -> 'area' = :area";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("area", NpgsqlDbType.Varchar));
                command.Parameters["area"].Value = area;
                var results = new List<string>();
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    results.Add(dataReader["name"].ToString());
                }

                return results;
            }
        }

        /// <summary>
        /// アーティスト名に対するタグ情報を取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<string> GetTagsByArtistName(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = "select kvs -> 'tags'  as tags from artist where kvs -> 'name' = :name";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));
                command.Parameters["name"].Value = name;
                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add(dataReader["tags"].ToString());
                }

                return results;
            }
        }
    }
}