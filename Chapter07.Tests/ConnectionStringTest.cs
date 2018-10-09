#region

using Chapter07.Core;
using NUnit.Framework;

#endregion

namespace Chapter07.Tests
{
    /// <summary>
    /// <seealso cref="ConnectionString"/>をテストします。
    /// </summary>
    [TestFixture]
    public class ConnectionStringTest
    {
        /// <summary>
        /// コンストラクタをテストします。
        /// </summary>
        [Test]
        public void Constructor()
        {
            var connectionString = new ConnectionString();
            Assert.NotNull(connectionString);
        }
    }
}