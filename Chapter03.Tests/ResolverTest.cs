using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter03.Core;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Chapter03.Tests
{
    [TestFixture]
    public class ResolverTest
    {
        /// <summary>
        /// <seealso cref="Resolver.Answer20"/>を検証します。
        /// </summary>
        [Test]
        public void Answer20()
        {
            Resolver resolver =  new Resolver();
            resolver.Answer20();
        }
        /// <summary>
        /// <seealso cref="Resolver.Answer21"/>を検証します。
        /// </summary>
        [Test]
        public void Answer21()
        {
            Resolver resolver = new Resolver();
            resolver.Answer21();
        }
        /// <summary>
        /// <seealso cref="Resolver.Answer22"/>を検証します。
        /// </summary>
        [Test]
        public void Answer22()
        {
            Resolver resolver = new Resolver();
            resolver.Answer22();
        }
    }
}
