using System;
using System.Collections.Generic;
using System.Linq;

namespace Q05
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string text = "I am an NLPer";
            Console.WriteLine("文字bi-gram");
            var result = CreateTextNgram(text, 2);
            Console.WriteLine(string.Join(" ", result));
            Console.WriteLine("単語bi-gram");
            var wordNgram = CreateWordNgram(text, 2);
            var wordNgramResult = "";
            foreach (var ngram in wordNgram)
            {
                string value = $"[{string.Join(",", ngram)}]";
                wordNgramResult += value + " ";

            }
            Console.WriteLine(wordNgramResult);
        }

        private static IEnumerable<IEnumerable<string>> CreateWordNgram(string text, int v)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            if (text.EndsWith("."))
            {
                text = text.Substring(0, text.Length);
            }
            var words = text.Split(' ');
            for (int i = 0; i <= words.Length - v; i++) {
                yield return words.Skip(i).Take(v);
            }
        }

        private static IList<string> CreateTextNgram(string text, int v)
        {
            if (text.EndsWith("."))
            {
                text = text.Substring(0, text.Length);
            }
            text = text.Replace(" ", string.Empty);
            var result = new List<string>();
            for (int i = 0; i < text.Length - v; i++) {
                result.Add(text.Substring(i, v));
            }
            return result;
        }
    }
}
