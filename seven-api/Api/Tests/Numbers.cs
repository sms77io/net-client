using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library.Numbers;

namespace Seven.Api.Tests
{
    [TestFixture]
    public class Numbers
    {
        [Test]
        public async Task ListAvailable()
        {
            var listAvailableParams = new ListAvailableParams
            {
                Country = "DE",
                FeaturesApplicationToPersonSms = true
            };
            var response = await BaseTest.Client.Numbers.ListAvailable(listAvailableParams);

            Assert.That(response.AvailableNumbers, Is.Not.Empty);

            foreach (var offer in response.AvailableNumbers)
            {
                Assert.AreEqual(listAvailableParams.Country, offer.Country);
                Assert.That(offer.Features.ApplicationToPersonSms, Is.True);
                Assert.That(offer.Fees.InbondSms, Is.GreaterThanOrEqualTo(0.0));
                Assert.That(offer.Fees.InboundVoice, Is.GreaterThanOrEqualTo(0.0));
                Assert.That(offer.Fees.Annually.BasicCharge, Is.GreaterThanOrEqualTo(0.0));
                Assert.That(offer.Fees.Annually.Setup, Is.GreaterThanOrEqualTo(0.0));
                Assert.That(offer.Fees.Monthly.BasicCharge, Is.GreaterThanOrEqualTo(0.0));
                Assert.That(offer.Fees.Monthly.Setup, Is.GreaterThanOrEqualTo(0.0));
                Assert.True(offer.Number.Length > 0);
                Assert.True(offer.NumberParsed.Length > 0);
            }
        }
        
        [Test]
        public async Task Order()
        {
            var listAvailableParams = new ListAvailableParams
            {
                Country = "DE",
                FeaturesApplicationToPersonSms = true
            };
            var availableNumbers = await BaseTest.Client.Numbers.ListAvailable(listAvailableParams);
            var availableNumber = availableNumbers.AvailableNumbers.First();

            var orderParams = new OrderParams(availableNumber)
            {
                PaymentInterval = PaymentInterval.Monthly
            };
            var orderResponse = await BaseTest.Client.Numbers.Order(orderParams);
            Assert.Null(orderResponse.Error);
            Assert.True(orderResponse.Success);

            var oneParams = new OneParams(availableNumber);
            var number = await BaseTest.Client.Numbers.One(oneParams);
            Assert.AreEqual(availableNumber.Number, number.Number);
            Assert.AreEqual(orderParams.PaymentInterval, number.Billing.PaymentInterval);
            
            var deleteParams = new DeleteParams(availableNumber.Number)
            {
                DeleteImmediately = true
            };
            var deleteResponse = await BaseTest.Client.Numbers.Delete(deleteParams);
            Assert.True(deleteResponse.Success);
            
            try {
                await BaseTest.Client.Numbers.One(oneParams);
                Assert.Fail();
            } catch (HttpRequestException) { }
        }
    }
}