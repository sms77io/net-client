using System;
using System.Linq;

namespace Seven.Api.Tests {
    internal static class TestHelper {
        internal static readonly string ApiKey = Environment.GetEnvironmentVariable("SEVEN_API_KEY");
        internal const string PhoneNumber = "+491771783130";
        internal static readonly string MyPhoneNumber = Environment.GetEnvironmentVariable("SEVEN_TO");

        internal static string CreateRandomUrl() {
            return $"http://net.tld/{Guid.NewGuid()}";
        }
        
        internal static string RandomString(int length = 16)
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}