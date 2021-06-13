using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Azenix.LogParser
{
    public class LogParser
    {
        private const string Pattern =
            @"(?<IpAddress>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b)\s{1}-\s{1}(?<IsAdmin>admin|-)\s{1}\[\d+/\w+/\d+:\d+:\d+:\d+\s{1}\+\d+\]\W+GET\s{1}(?<Url>[\/\.:\w-]+)\s{1}HTTP/1.1\W+(?<Status>\d{3})";
        public ParseResult Parse(string input)
        {
            var requests = ExtractRequests(input);
            return new ParseResult
            {
                UniqueIpAddressesCount = requests.Select(x => x.IpAddress).Distinct().Count(),
                Top3MostActiveIpAddresses =
                    requests
                        .Where(x =>
                            x.Status != HttpStatusCode.MovedPermanently &&
                            x.Status != HttpStatusCode.TemporaryRedirect)
                        .GetTop3ByStringKey(x => x.IpAddress),
                Top3MostVisitedUrls =
                    requests
                        .Where(x =>
                            x.Status != HttpStatusCode.MovedPermanently &&
                            x.Status != HttpStatusCode.TemporaryRedirect &&
                            x.Status != HttpStatusCode.NotFound &&
                            x.Status != HttpStatusCode.InternalServerError)
                        .GetTop3ByStringKey(x => x.Url)
            };
        }

        private IReadOnlyList<RequestDetails> ExtractRequests(string input)
        {
            var requests = new List<RequestDetails>();
            var regex = new Regex(Pattern);
            var matches = regex.Matches(input);
            var ipAddress = "";
            var isAdmin = false;
            var url = "";
            var status = HttpStatusCode.Accepted;

            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    switch (group.Name)
                    {
                        case "IpAddress":
                            ipAddress = group.Value;
                            break;
                        case "IsAdmin":
                            isAdmin = group.Value.Equals("admin");
                            break;
                        case "Url":
                            url = group.Value;
                            break;
                        case "Status":
                            status = (HttpStatusCode)int.Parse(group.Value);
                            break;
                    }
                }

                requests.Add(new RequestDetails
                {
                    IpAddress = ipAddress,
                    IsAdmin = isAdmin,
                    Url = url,
                    Status = status
                });
            }

            return requests;
        }
    }
}
