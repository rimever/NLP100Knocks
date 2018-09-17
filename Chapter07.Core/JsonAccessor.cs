﻿using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;

namespace Chapter07.Core
{
    /// <summary>
    /// Jsonデータへのアクセスを行うクラスです。
    /// </summary>
    public class JsonAccessor
    {
        private readonly ConnectionString _connectionString = new ConnectionString();

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
    }
}