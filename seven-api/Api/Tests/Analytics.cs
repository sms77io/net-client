using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Analytics;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Analytics {
        private static void AssertBase(AnalyticsBase entry)
        {
            Assert.That(entry.Hlr, Is.Not.Negative);
            Assert.That(entry.Inbound, Is.Not.Negative);
            Assert.That(entry.Mnp, Is.Not.Negative);
            Assert.That(entry.Sms, Is.Not.Negative);
            Assert.That(entry.UsageEur, Is.Not.Negative);
            Assert.That(entry.Voice, Is.Not.Negative);
        }
        
        [Test]
        public async Task ByCountry()
        {
            var entries = await BaseTest.Client.Analytics.ByCountry(new AnalyticsParams
            {
                End = null,
                Label = null,
                Start = null,
                Subaccounts = null
            });
            foreach (var entry in entries) 
            {
                Assert.True(entry.Country == "" || entry.Country.Length == 2);
                AssertBase(entry);
            }
        }
        
        [Test]
        public async Task ByDate()
        {
            var entries = await BaseTest.Client.Analytics.ByDate();
            foreach (var entry in entries) 
            {
                Assert.That(entry.Date, Is.Not.Empty);
                AssertBase(entry);
            }
        }
        
        [Test]
        public async Task ByLabel()
        {
            var entries = await BaseTest.Client.Analytics.ByLabel();
            const string pattern = @"[a-zA-Z0-9\\-_@.]+";
            foreach (var entry in entries)
            {
                var match = Regex.Match(entry.Label, pattern);
                Assert.AreEqual(entry.Label, match.Value);
                //Assert.True(Regex.IsMatch(entry.Label, pattern));
                Assert.True(entry.Label.Length <= 100);
                AssertBase(entry);
            }
        }
        
        [Test]
        public async Task BySubaccount()
        {
            var entries = await BaseTest.Client.Analytics.BySubaccount();
            foreach (var entry in entries)
            {
                Assert.True(entry.Account.Length > 0);
                AssertBase(entry);
            }
        }
    }
}