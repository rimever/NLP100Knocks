#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace Chapter05.Core
{
    public class CabochaAnalyzer
    {
        private static readonly string CabochaFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Chapter05.Core\neko.txt.cabocha");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CabochaAnalyzer()
        {
            Debug.Assert(File.Exists(CabochaFileName), Path.GetFullPath(CabochaFileName));
        }

        public IList<Sentence> Sentences { get; set; } = new List<Sentence>();

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute()
        {
            Sentences = EnumerableSentences().ToList();
        }

        private IEnumerable<Sentence> EnumerableSentences()
        {
            var xml = XDocument.Load(CabochaFileName);
            foreach (var sentence in xml.Root.Elements("sentence"))
            {
                List<Chunk> list = EnumerableChunk(sentence).ToList();
                foreach (var chunk in list)
                {
                    chunk.Srcs = list.Where(c => c.Dst == chunk.Id).Select(c => c.Id).ToList();
                }

                yield return new Sentence
                {
                    Chunks = list
                };
            }
        }

        private IEnumerable<Chunk> EnumerableChunk(XElement sentence)
        {
            foreach (var chunk in sentence.Elements("chunk"))
            {
                var morphs = EnumerableMorphs(chunk).ToList();
                yield return new Chunk
                {
                    Id = int.Parse(chunk.Attribute("id").Value),
                    Dst = int.Parse(chunk.Attribute("link").Value),
                    Morphs = morphs
                };
            }
        }

        private IEnumerable<Morph> EnumerableMorphs(XElement chunk)
        {
            foreach (var token in chunk.Elements("tok"))
            {
                var features = token.Attribute("feature").Value.Split(',');
                yield return new Morph
                {
                    Id = int.Parse(token.Attribute("id").Value),
                    Surface = token.Value,
                    Base = features[6],
                    Pos = features[0],
                    Pos1 = features[1]
                };
            }
        }
    }
}