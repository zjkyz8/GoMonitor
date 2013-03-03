using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using GoMonitor;
using NUnit.Framework;

namespace GoMonitor_tests
{
    public class ContentProviderFacts
    {
        [Test]
        public void should_get_file_file_go_server()
        {
            new ContentProvider().GetContent("http://10.18.7.153:8153/go/cctray.xml");

            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml", SearchOption.TopDirectoryOnly);

            var matchedfiles = files.Where(x => Regex.IsMatch(Path.GetFileNameWithoutExtension(x), "^[0-9]*$")).Select(x=>x).ToList();
            matchedfiles.Count().Should().BeGreaterOrEqualTo(1);

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}
