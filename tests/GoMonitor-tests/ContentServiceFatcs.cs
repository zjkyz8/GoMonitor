using GoMonitor;
using NUnit.Framework;

namespace GoMonitor_tests
{
    public class ContentServiceFatcs
    {
        [Test]
        public void should_return_list()
        {
            var xmlNodeList = new ContentService().GetFormatedContent();
        }
    }
}