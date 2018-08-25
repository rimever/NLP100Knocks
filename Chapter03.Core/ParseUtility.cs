using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter03.Core
{
    public static class ParseUtility
    {
        private const string StartDoubleBrace = "{{";
        private const string EndDoubleBrace = "}}";
        private const string StartDoubleBracket = "[[";
        private const string EndDoubleBracket = "]]";
        private const string StartSingleBracket = "[";
        private const string EndSingleBracket = "]";
        private const string Separator = "|";

        public static IList<string> ParseBetweenBrace(string text)
        {
            var results = new List<string>();
            int bracketCount = 0;
            int firstBracketIndex = -1;
            for (int nowIndex = 0; nowIndex < text.Length; nowIndex++)
            {
                if (text.Substring(nowIndex).StartsWith(StartDoubleBrace))
                {
                    if (bracketCount == 0)
                    {
                        firstBracketIndex = nowIndex + StartDoubleBrace.Length;
                    }

                    bracketCount++;
                    continue;
                }

                if (text.Substring(nowIndex).StartsWith(EndDoubleBrace))
                {
                    bracketCount--;
                    if (bracketCount == 0)
                    {
                        string between = text.Substring(firstBracketIndex, nowIndex - firstBracketIndex);
                        results.Add(between);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ParseKeyValue(string text)
        {
            var results = new Dictionary<string, string>();
            int braceCount = 0;
            int firstSeparatorIndex = -1;
            int bracketCount = 0;


            void StoreDictionary(string keyAndValue)
            {
                // a = <ref name = test>というケースがあるのでsplitしない
                var index = keyAndValue.IndexOf("=", StringComparison.Ordinal);
                results.Add(keyAndValue.Substring(0, index).Trim(),
                    keyAndValue.Substring(index + 1, keyAndValue.Length - index - 1).Trim());
            }

            for (int nowIndex = 0; nowIndex < text.Length; nowIndex++)
            {
                string rest = text.Substring(nowIndex);
                if (rest.StartsWith(Separator))
                {
                    if (braceCount == 0
                        && bracketCount == 0)
                    {
                        if (firstSeparatorIndex != -1)
                        {
                            string between = text.Substring(firstSeparatorIndex, nowIndex - firstSeparatorIndex);
                            StoreDictionary(between);
                        }

                        firstSeparatorIndex = nowIndex + Separator.Length;
                    }
                }
                else if (rest.StartsWith(StartDoubleBrace))
                {
                    braceCount++;
                }
                else if (rest.StartsWith(EndDoubleBrace))
                {
                    braceCount--;
                }
                else if (rest.StartsWith(StartDoubleBracket))
                {
                    bracketCount++;
                }
                else if (rest.StartsWith(EndDoubleBracket))
                {
                    bracketCount--;
                }
            }

            StoreDictionary(text.Substring(firstSeparatorIndex));
            return results;
        }

        /// <summary>
        /// 強調表現を表すマークアップを取り除いて、テキストに変換
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveStrongMarkup(string text)
        {
            return text.Replace("'", String.Empty);
        }

        /// <summary>
        /// 内部リンクのマークアップを取り除く
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveInnerLinkMarkup(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bracketStartIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                string rest = text.Substring(i);
                if (rest.StartsWith(StartDoubleBracket))
                {
                    bracketStartIndex = i;
                    i += StartDoubleBracket.Length - 1;
                }
                else if (rest.StartsWith(EndDoubleBracket))
                {
                    string value = text.Substring(bracketStartIndex + StartDoubleBracket.Length,
                        i - bracketStartIndex - StartDoubleBracket.Length);
                    int sepratorIndex = value.IndexOf(Separator, StringComparison.Ordinal);
                    if (sepratorIndex != -1)
                    {
                        value = value.Substring(sepratorIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketStartIndex = -1;
                    i += EndDoubleBracket.Length - 1;
                }
                else if (bracketStartIndex == -1)
                {
                    stringBuilder.Append(text[i]);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// メディアリンクを取り除く
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveMediaLinkMarkup(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bracketDoubleStartIndex = -1;
            int bracketSingleStartIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                string rest = text.Substring(i);
                if (rest.StartsWith(StartDoubleBracket))
                {
                    bracketDoubleStartIndex = i;
                    i += StartDoubleBracket.Length - 1;
                }
                else if (rest.StartsWith(EndDoubleBracket))
                {
                    string value = text.Substring(bracketDoubleStartIndex + StartDoubleBracket.Length,
                        i - bracketDoubleStartIndex - StartDoubleBracket.Length);
                    int sepratorIndex = value.LastIndexOf(Separator, StringComparison.Ordinal);
                    if (sepratorIndex != -1)
                    {
                        value = value.Substring(sepratorIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketDoubleStartIndex = -1;
                    i += EndDoubleBracket.Length - 1;
                }
                else if (rest.StartsWith(StartSingleBracket))
                {
                    bracketSingleStartIndex = i;
                    i += StartSingleBracket.Length - 1;
                }
                else if (rest.StartsWith(EndSingleBracket))
                {
                    string value = text.Substring(bracketSingleStartIndex + StartSingleBracket.Length,
                        i - bracketSingleStartIndex - StartSingleBracket.Length);
                    int spaceIndex = value.IndexOf(" ", StringComparison.Ordinal);
                    if (spaceIndex != -1)
                    {
                        value = value.Substring(spaceIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketSingleStartIndex = -1;
                    i += EndSingleBracket.Length - 1;
                }
                else if (bracketDoubleStartIndex == -1
                         && bracketSingleStartIndex == -1)
                {
                    stringBuilder.Append(text[i]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}