using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter03.Core
{
    public static class ParseUtility
    {
        private const string StartBrace = "{{";
        private const string EndBrace = "}}";
        private const string StartBracket = "[[";
        private const string EndBracket = "]]";
        private const string Separator = "|";

        public static IList<string> ParseBetweenBrace(string text)
        {
            var results = new List<string>();
            int bracketCount = 0;
            int firstBracketIndex = -1;
            for (int nowIndex = 0; nowIndex < text.Length; nowIndex++)
            {
                if (text.Substring(nowIndex).StartsWith(StartBrace))
                {
                    if (bracketCount == 0)
                    {
                        firstBracketIndex = nowIndex + StartBrace.Length;
                    }

                    bracketCount++;
                    continue;
                }

                if (text.Substring(nowIndex).StartsWith(EndBrace))
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
                else if (rest.StartsWith(StartBrace))
                {
                    braceCount++;
                }
                else if (rest.StartsWith(EndBrace))
                {
                    braceCount--;
                }
                else if (rest.StartsWith(StartBracket))
                {
                    bracketCount++;
                }
                else if (rest.StartsWith(EndBracket))
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

        public static string RemoveInnerLinkMarkup(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int bracketStartIndex = -1;
            for (int i = 0; i < text.Length; i++)
            {
                string rest = text.Substring(i);
                if (rest.StartsWith(StartBracket))
                {
                    bracketStartIndex = i;
                    i += StartBracket.Length - 1;
                }
                else if (rest.StartsWith(EndBracket))
                {
                    string value = text.Substring(bracketStartIndex + StartBracket.Length,
                        i - bracketStartIndex - StartBracket.Length);
                    int sepratorIndex = value.IndexOf(Separator, StringComparison.Ordinal);
                    if (sepratorIndex != -1)
                    {
                        value = value.Substring(sepratorIndex + 1);
                    }

                    stringBuilder.Append(value);
                    bracketStartIndex = -1;
                    i += EndBracket.Length - 1;
                }
                else if (bracketStartIndex == -1)
                {
                    stringBuilder.Append(text[i]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}