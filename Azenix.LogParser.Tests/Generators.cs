using System;
using System.Net;

namespace Azenix.LogParser.Tests
{
    class Generators
    {
        public static string GetRandomIpAddress()
        {
            var data = new byte[4];
            new Random().NextBytes(data);
            return new IPAddress(data).ToString();
        }
    }
}