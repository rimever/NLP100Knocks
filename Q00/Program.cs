using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q00
{
    class Program
    {
        /// <summary>
        /// 00. 文字列の逆順
        /// 文字列"stressed"の文字を逆に（末尾から先頭に向かって）並べた文字列を得よ．
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string text = "stressed";
            Console.WriteLine(string.Join("",text.Reverse()));
            Console.ReadKey();
        }
    }
}
