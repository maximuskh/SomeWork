using System;
using System.Linq;
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

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}