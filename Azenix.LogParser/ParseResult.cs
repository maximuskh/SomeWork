namespace Azenix.LogParser
{
    public class ParseResult
    {
        public int UniqueIpAddressesCount { get; set; }
        public string[] Top3MostVisitedUrls { get; set; }
        public string[] Top3MostActiveIpAddresses { get; set; }
    }
}