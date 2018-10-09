#region

using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;

#endregion

namespace Chapter07.Core
{
    /// <summary>
    /// Jsonデータへのアクセスを行うクラスです。
    /// </summary>
    public class JsonAccessor
    {
        private readonly ConnectionString _connectionString;

        public JsonAccessor(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// アーティスト名でレコードを取得します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<string> GetRecordsByArtistName(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = "select json from artist where json->>'name' = :name";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));
                command.Parameters["name"].Value = name;
                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add(dataReader["json"].ToString());
                }

                return results;
            }
        }

        /// <summary>
        /// 指定の活動場所に該当するデータを取得します。
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IList<string> GetRecordsByArea(string area)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = "select json from artist where json->>'area' = :area";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("area", NpgsqlDbType.Varchar));
                command.Parameters["area"].Value = area;
                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add(dataReader["json"].ToString());
                }

                return results;
            }
        }

        /// <summary>
        /// 指定された別名のアーティストを取得します。
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public IList<string> GetRecordsByAlias(string alias)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = @"select json from artist where json->>'aliases' LIKE :alias";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.Add(new NpgsqlParameter("alias", NpgsqlDbType.Varchar));
                command.Parameters["alias"].Value = $@"%\""name\""%:%\""{alias}\""%";
                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add(dataReader["json"].ToString());
                }

                return results;
            }
        }

        /// <summary>
        /// ダンスのタグを持つアーティストをレーティング投票数の多い順で並び替えた結果を取得します。
        /// </summary>
        /// <returns></returns>
        public IList<string> GetRecordsOrderByRatingCountWithDanceTag()
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql =
                    @"select json from artist where json->>'tags' LIKE '%\""value\""%:%\""dance\""%' ORDER BY COALESCE(json#>>'{rating,count}','-1')::int DESC";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add(dataReader["json"].ToString());
                }

                return results;
            }
        }

        public IList<string> GetRecords(string keyword, bool isArtist, bool isAlias, bool isTags)
        {
            using (var connection = new NpgsqlConnection(_connectionString.Value))
            {
                connection.Open();
                string sql = @"select json from artist where 1 = 0 ";
                if (isArtist)
                {
                    sql += " OR json->>'name' LIKE :name";
                }

                if (isAlias)
                {
                    sql += " OR json->>'aliases' LIKE :alias";
                }

                if (isTags)
                {
                    sql += " OR  json->>'tags' LIKE :tags";
                }

                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                if (isArtist)
                {
                    command.Parameters.Add(new NpgsqlParameter("name", NpgsqlDbType.Varchar));
                    command.Parameters["name"].Value = $@"%{keyword}%";
                }

                if (isAlias)
                {
                    command.Parameters.Add(new NpgsqlParameter("alias", NpgsqlDbType.Varchar));
                    command.Parameters["alias"].Value = $@"%\""name\""%:%\""{keyword}\""%";
                }

                if (isTags)
                {
                    command.Parameters.Add(new NpgsqlParameter("tags", NpgsqlDbType.Varchar));
                    command.Parameters["tags"].Value = $@"%\""value\""%:%\""%{keyword}%\""%";
                }

                var dataReader = command.ExecuteReader();
                var results = new List<string>();
                while (dataReader.Read())
                {
                    results.Add(dataReader["json"].ToString());
                }

                return results;
            }
        }
    }
}