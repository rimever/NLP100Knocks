using System.IO;

namespace Chapter09.Core
{
    public class AnswerService
    {
        private const string RootDir = @"../../../../Chapter09.Core";

        public readonly string _dataSourceFilePath =
            Path.GetFullPath(Path.Combine(RootDir, @"DataSource", "enwiki-20150112-400-r100-10576.txt"));

    }
}