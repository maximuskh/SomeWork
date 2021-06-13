using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Azenix.LogParser.Tests
{
    public class LogParserTests
    {
        private readonly LogParser _logParser;

        public LogParserTests()
        {
            _logParser = new LogParser();
        }

        [Fact]
        public async Task Given_ValidLogFile_LogIsParsedSuccessfully()
        {
            // Arrange
            var logFile = Path.Combine(Environment.CurrentDirectory, "ValidLogFile.log");
            var input = await File.ReadAllTextAsync(logFile);

            // Action

            var result = _logParser.Parse(input);

            // Assert
            result.UniqueIpAddressesCount.ShouldBe(11);

            result.Top3MostActiveIpAddresses.Length.ShouldBe(3);
            result.Top3MostActiveIpAddresses[0].ShouldBe("168.41.191.40");
            result.Top3MostActiveIpAddresses[1].ShouldBe("177.71.128.21");
            result.Top3MostActiveIpAddresses[2].ShouldBe("50.112.00.11");

            result.Top3MostVisitedUrls.Length.ShouldBe(3);
            result.Top3MostVisitedUrls[0].ShouldBe("/docs/manage-websites/");
            result.Top3MostVisitedUrls[1].ShouldBe("/intranet-analytics/");
            result.Top3MostVisitedUrls[2].ShouldBe("http://example.net/faq/");
        }

        [Fact]
        public void Given_RepeatedIpAddresses_UniqueIpAddressesCount_ShouldConsiderUniqueIpAddresses()
        {
            // Arrange
            var url = "test.com";
            var ip1 = Generators.GetRandomIpAddress();
            var ip2 = Generators.GetRandomIpAddress();
            var ip3 = Generators.GetRandomIpAddress();
            var ips = new[] { ip1, ip2, ip3 };

            var logFile = new StringBuilder();
            foreach (var ip in ips)
            {
                logFile.Append(GenerateLogRecord(ip, HttpStatusCode.OK, url, 10));
            }

            // Action
            var result = _logParser.Parse(logFile.ToString());

            // Assert
            result.UniqueIpAddressesCount.ShouldBe(3);
        }

        [Fact]
        public void Top3MostActiveIpAddresses_ShouldNotConsider_MovedPermanently_Or_TemporaryRedirect_Logs_ByOrderingDescing()
        {
            // Arrange
            var url = "test.com";
            var logFile = new StringBuilder();

            // Valid record counts 5
            // Record counts 15
            var ip1 = Generators.GetRandomIpAddress();
            logFile.Append(GenerateLogRecord(ip1, HttpStatusCode.MovedPermanently, url, 5));
            logFile.Append(GenerateLogRecord(ip1, HttpStatusCode.TemporaryRedirect, url, 5));
            logFile.Append(GenerateLogRecord(ip1, HttpStatusCode.OK, url, 5));

            // Valid record counts 10
            // Record counts 10
            var ip2 = Generators.GetRandomIpAddress();
            logFile.Append(GenerateLogRecord(ip2, HttpStatusCode.OK, url, 5));
            logFile.Append(GenerateLogRecord(ip2, HttpStatusCode.NotFound, url, 5));


            // Record counts 7
            var ip3 = Generators.GetRandomIpAddress();
            logFile.Append(GenerateLogRecord(ip3, HttpStatusCode.OK, url, 7));

            // Action
            var result = _logParser.Parse(logFile.ToString());

            // Assert
            result.Top3MostActiveIpAddresses[0].ShouldBe(ip2);
            result.Top3MostActiveIpAddresses[1].ShouldBe(ip3);
            result.Top3MostActiveIpAddresses[2].ShouldBe(ip1);
        }

        private string GenerateLogRecord(string ipAddress, HttpStatusCode status, string url, int count)
        {
            var logRecords = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                var logRecord = new LogRecordBuilder()
                    .WithIpAddress(ipAddress)
                    .WithUrl(url)
                    .WithStatusCode(status)
                    .Build();

                logRecords.Append(logRecord);
                logRecords.Append(Environment.NewLine);
            }

            return logRecords.ToString();
        }
    }
}
