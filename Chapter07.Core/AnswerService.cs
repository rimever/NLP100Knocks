using System;
using System.Linq;

namespace Chapter07.Core
{
    /// <summary>
    /// artist.json.gzは，オープンな音楽データベースMusicBrainzの中で，アーティストに関するものをJSON形式に変換し，gzip形式で圧縮したファイルである．このファイルには，1アーティストに関する情報が1行にJSON形式で格納されている．JSON形式の概要は以下の通りである．
    /// </summary>
    public class AnswerService
    {
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
            JsonAccessor accessor = new JsonAccessor();
            foreach (var json in accessor.GetRecordsByArtistName(name))
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
            JsonAccessor accessor = new JsonAccessor();
            int count = accessor.GetRecordsByArea(area).Count;
            Console.WriteLine($"{area}:{count}件");
        }
        /// <summary>
        /// 67. 複数のドキュメントの取得
        /// 特定の（指定した）別名を持つアーティストを検索せよ．
        /// </summary>
        /// <param name="aliases"></param>
        public void Answer67(string aliases)
        {
            JsonAccessor accessor = new JsonAccessor();
            foreach (var json in accessor.GetRecordsByAlias(aliases))
            {
                Console.WriteLine(json);
            }

        }
    }
}