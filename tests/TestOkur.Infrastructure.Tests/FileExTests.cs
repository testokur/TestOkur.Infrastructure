using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestOkur.Infrastructure.Mvc.Helpers;
using Xunit;

namespace TestOkur.Infrastructure.Tests
{
    public class FileExTests
    {
        [Fact]
        public async Task ShouldReadAllText()
        {
            const string filePath = "dummy.txt";

            await CreateADummyFileIfNotExists(filePath);
            var text = await FileEx.ReadAllTextAsync(filePath);
            text.Should().BeEquivalentTo(File.ReadAllText(filePath));
        }

        private Task CreateADummyFileIfNotExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                return Task.CompletedTask;
            }
            var lines = new List<string>();

            for (var i = 0; i < 1000; i++)
            {
                lines.Add(Guid.NewGuid().ToString("N"));
            }

            return File.WriteAllLinesAsync(filePath, lines);
        }
    }
}
