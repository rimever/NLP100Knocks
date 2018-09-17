using System;
using System.Diagnostics;
using System.IO;

namespace Chapter07.Core
{
    /// <summary>
    /// 接続識別子を扱うクラスです。
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConnectionString(string relativePath = @"..\..\..\Chapter07.Core")
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                relativePath, "connectionString.txt"));
            Debug.Assert(File.Exists(path), $"接続識別子を宣言する。{path}が存在しません。ファイルを作成してください。");
            Value = File.ReadAllText(path);
        }

        /// <summary>
        /// 接続識別子
        /// </summary>
        public string Value { get; }
    }
}