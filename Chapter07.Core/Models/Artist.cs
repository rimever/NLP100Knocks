#region

using Newtonsoft.Json.Linq;

#endregion

namespace Chapter07.Core.Models
{
    /// <summary>
    /// アーティスト情報を扱うクラスです。
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="jObject"></param>
        public Artist(JObject jObject)
        {
            Name = TryGetString(jObject, "name");
            Area = TryGetString(jObject, "area");
            BeginYear = GetBeginYear(jObject);
        }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 活動開始年
        /// </summary>
        /// <remarks>
        /// 月日の指定されているケースもあるが、表示することはなさそうなので年のみ
        /// </remarks>
        public int BeginYear { get; set; }

        /// <summary>
        /// 活動場所
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 指定されたプロパティの値を取得します。
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string TryGetString(JObject jObject, string propertyName)
        {
            if (jObject.TryGetValue(propertyName, out var outToken))
            {
                return outToken.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 活動開始日時を取得します。
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        private int GetBeginYear(JObject jObject)
        {
            if (jObject.TryGetValue("begin", out var outToken))
            {
                return int.Parse(outToken["year"].ToString());
            }

            return 0;
        }
    }
}