using System;
using System.IO;
using System.Threading.Tasks;

namespace Azenix.LogParserConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide filename as first argument");
                Console.ReadKey();
                return;
            }
            var filePath = Path.GetFullPath(args[0]);
            if (File.Exists(filePath))
            {
                var input = await File.ReadAllTextAsync(filePath);
                var parseResult = new LogParser.LogParser().Parse(input);
                var newLine = Environment.NewLine;
                Console.WriteLine($"*****Unique IP Addresses Count*****{newLine}{parseResult.UniqueIpAddressesCount}");
                Console.WriteLine();
                Console.WriteLine($"*****Top 3 Most Visited Urls*****{newLine}{string.Join(newLine, parseResult.Top3MostVisitedUrls)}");
                Console.WriteLine();
                Console.WriteLine($"*****Top 3 Most Active IP Addresses*****{newLine}{string.Join(newLine, parseResult.Top3MostActiveIpAddresses)}");
            }
            else
            {
                Console.WriteLine($"File {filePath} does not exist");
            }
            Console.WriteLine("Press any key to close!");
            Console.ReadKey();
        }
    }
}
