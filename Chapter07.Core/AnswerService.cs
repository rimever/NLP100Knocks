using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter07.Core
{
    /// <summary>
    /// artist.json.gzは，オープンな音楽データベースMusicBrainzの中で，アーティストに関するものをJSON形式に変換し，gzip形式で圧縮したファイルである．このファイルには，1アーティストに関する情報が1行にJSON形式で格納されている．JSON形式の概要は以下の通りである．
    /// </summary>
    public class AnswerService
    {
        private readonly JsonAccessor _jsonAccessor;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString"></param>
        public AnswerService(ConnectionString connectionString)
        {
            _jsonAccessor = new JsonAccessor(connectionString);
        }

        /// <summary>
        /// 61. KVSの検索
        /// 60で構築したデータベースを用い，特定の（指定された）アーティストの活動場所を取得せよ．
        /// </summary>
        /// <param name="name"></param>
        public void Answer61(string name)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var area in accessor.GetAreaByArtistName(name).ToList())
            {
                Console.WriteLine(area);
            }
        }

        /// <summary>
        /// 62. KVS内の反復処理
        /// 60で構築したデータベースを用い，活動場所が「Japan」となっているアーティスト数を求めよ．
        /// </summary>
        public void Answer62()
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var name in accessor.GetArtistNameByArea("Japan").ToList())
            {
                Console.WriteLine(name);
            }
        }

        /// <summary>
        /// 63. オブジェクトを値に格納したKVS
        /// KVSを用い，アーティスト名（name）からタグと被タグ数（タグ付けされた回数）のリストを検索するためのデータベースを構築せよ．さらに，ここで構築したデータベースを用い，アーティスト名からタグと被タグ数を検索せよ．
        /// </summary>
        /// <param name="name"></param>
        public void Answer63(string name)
        {
            KeyValueStoreAccessor accessor = new KeyValueStoreAccessor();
            foreach (var tag in accessor.GetTagsByArtistName(name).ToList())
            {
                Console.WriteLine(tag);
            }
        }

        /// <summary>
        /// 65. MongoDBの検索
        /// MongoDBのインタラクティブシェルを用いて，"Queen"というアーティストに関する情報を取得せよ．さらに，これと同様の処理を行うプログラムを実装せよ．
        /// </summary>
        /// <param name="name"></param>
        public void Answer65(string name)
        {
            foreach (var json in _jsonAccessor.GetRecordsByArtistName(name))
            {
                Console.WriteLine(json);
            }
        }

        /// <summary>
        /// 65. MongoDBの検索
        /// MongoDBのインタラクティブシェルを用いて，"Queen"というアーティストに関する情報を取得せよ．さらに，これと同様の処理を行うプログラムを実装せよ．
        /// </summary>
        /// <param name="area"></param>
        public void Answer66(string area)
        {
            int count = _jsonAccessor.GetRecordsByArea(area).Count;
            Console.WriteLine($"{area}:{count}件");
        }
        /// <summary>
        /// 67. 複数のドキュメントの取得
        /// 特定の（指定した）別名を持つアーティストを検索せよ．
        /// </summary>
        /// <param name="aliases"></param>
        public void Answer67(string aliases)
        {
            foreach (var json in _jsonAccessor.GetRecordsByAlias(aliases))
            {
                Console.WriteLine(json);
            }

        }
        /// <summary>
        /// 68. ソート
        /// "dance"というタグを付与されたアーティストの中でレーティングの投票数が多いアーティスト・トップ10を求めよ．
        /// </summary>
        public void Answer68()
        {
            foreach (var json in _jsonAccessor.GetRecordsOrderByRatingCountWithDanceTag().Take(10))
            {
                Console.WriteLine(json);
            }
        }
        /// <summary>
        /// 69. Webアプリケーションの作成
        /// ユーザから入力された検索条件に合致するアーティストの情報を表示するWebアプリケーションを作成せよ．アーティスト名，アーティストの別名，タグ等で検索条件を指定し，アーティスト情報のリストをレーティングの高い順などで整列して表示せよ．
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="isArtist"></param>
        /// <param name="isAlias"></param>
        /// <param name="isTags"></param>
        /// <returns></returns>
        public IList<string> Answer69(string keyword, bool isArtist, bool isAlias, bool isTags)
        {
            return _jsonAccessor.GetRecords(keyword, isArtist, isAlias, isTags);
        }
    }
}