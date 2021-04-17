using System;
using Xunit;

namespace Elgator.UnitTests
{
    public class ElgatorTests
    {
        [Fact]
        public void ThrowsOnNullConfiguration()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Elgator.FromConfiguration(null));
            Assert.Contains("configuration", result.Message);
        }
    }
}