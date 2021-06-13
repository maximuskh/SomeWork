using System.Net;

namespace Azenix.LogParser.Tests
{
    class LogRecordBuilder
    {
        private string _ipAddress;
        private string _url;
        private HttpStatusCode _statusCode;

        public LogRecordBuilder WithIpAddress(string ipAddress)
        {
            _ipAddress = ipAddress;
            return this;
        }

        public LogRecordBuilder WithUrl(string url)
        {
            _url = url;
            return this;
        }

        public LogRecordBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
            return this;
        }

        public string Build()
        {
            return $"{_ipAddress} - - [10/Jul/2018:22:21:28 +0200] \"GET {_url} HTTP/1.1\" {(int)_statusCode} 3574 \"-\" \"Mozilla /5.0(X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7(KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7\"";
        }
    }
}
