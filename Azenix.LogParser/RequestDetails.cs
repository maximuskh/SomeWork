﻿using System.Net;

namespace Azenix.LogParser
{
    class RequestDetails
    {
        public string IpAddress { get; set; }
        public bool IsAdmin { get; set; }
        public string Url { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}