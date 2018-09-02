namespace Chapter05.Core
{
    /// <summary>
    /// 形態素
    /// </summary>
    public class Morph
    {
        public const string SignPosName = "記号";

        /// <summary>
        /// 形態素番号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 表層形
        /// </summary>
        public string Surface { get; set; }

        /// <summary>
        /// 基本形
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// 品詞
        /// </summary>
        public string Pos { get; set; }

        /// <summary>
        /// 品詞細分類1
        /// </summary>
        public string Pos1 { get; set; }
    }
}