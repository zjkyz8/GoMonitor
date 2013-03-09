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
        private ContentProvider contentProvider;

        [SetUp]
        public void Setup()
        {
            contentProvider = new ContentProvider();
        }

        [Test]
        public void should_get_file_file_go_server()
        {
            var content = contentProvider.GetContent("http://10.18.7.153:8153/go/cctray.xml");

            content.Length.Should().NotBe(0);
        }

        [Test]
        public void should_write_file_with_name()
        {
            var fileName = contentProvider.WriteContent(new byte[] {1, 2, 3, 4, 6, 43, 3, 5, 35, 213, 211});

            Regex.IsMatch(Path.GetFileNameWithoutExtension(fileName), "^[0-9]*$");
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
