using System;

namespace Seven.Api.Tests {
    internal static class TestHelper {
        internal static readonly string ApiKey = Environment.GetEnvironmentVariable("SEVEN_API_KEY");
        internal const string PhoneNumber = "+491771783130";
        internal static readonly string MyPhoneNumber = Environment.GetEnvironmentVariable("SEVEN_TO");

        internal static string CreateRandomUrl() {
            return $"http://net.tld/{Guid.NewGuid()}";
        }
    }
}