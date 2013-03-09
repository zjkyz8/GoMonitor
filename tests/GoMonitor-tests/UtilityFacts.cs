using FluentAssertions;
using NUnit.Framework;

namespace GoMonitor_tests
{
    public class UtilityFacts
    {
        [Test]
        public void should_get_md5_for_a_string()
        {
            Utility.CalculateContentMD5(
                System.Text.Encoding.Default.GetBytes("this is a demo test, to make sure MD5 algo correct!")).Should().
                BeEquivalentTo(
                    "4337fa2d873d17e9cce2424b87c110bf");
        }
    }
}