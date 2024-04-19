using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library.Pricing;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Pricing {
        [Test]
        public async Task TestPricingGlobal() {
            var response = await BaseTest.Client.Pricing.Get();

            Assert.That(response.CountCountries, Is.Positive);
            Assert.AreEqual(response.CountCountries, response.Countries.Count);
            Assert.That(response.CountNetworks, Is.Positive);
        }

        [Test]
        public async Task TestPricingGermany() {
            var response = await BaseTest.Client.Pricing.Get(new PricingParams {Country = "de"});

            Assert.That(response.CountCountries, Is.EqualTo(1));
        }
    }
}