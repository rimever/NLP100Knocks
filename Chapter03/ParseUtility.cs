using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter03
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
                var splits = keyAndValue.Split('=');
                results.Add(splits[0].Trim(), splits[1].Trim());
            }

            for (int nowIndex = 0; nowIndex < text.Length; nowIndex++)
            {
                if (text.Substring(nowIndex).StartsWith(Separator))
                {
                    if (braceCount == 0 && bracketCount == 0)
                    {
                        if (firstSeparatorIndex != -1)
                        {
                            string between = text.Substring(firstSeparatorIndex, nowIndex - firstSeparatorIndex);
                            StoreDictionary(between);

                        }
                        firstSeparatorIndex = nowIndex + Separator.Length;
                    }
                }
                else if (text.Substring(nowIndex).StartsWith(StartBrace))
                {
                    braceCount++;
                }
                else if (text.Substring(nowIndex).StartsWith(EndBrace))
                {
                    braceCount--;
                }
                else if (text.Substring(nowIndex).StartsWith(StartBracket))
                {
                    bracketCount++;
                }
                else if (text.Substring(nowIndex).StartsWith(EndBracket))
                {
                    bracketCount--;
                }
            }

            StoreDictionary(text.Substring(firstSeparatorIndex));
            return results;
        }
    }
}
