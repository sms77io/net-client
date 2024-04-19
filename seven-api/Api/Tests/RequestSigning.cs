using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Sms;

namespace Seven.Api.Tests {
    [TestFixture]
    public class RequestSigning {
        [Test]
        public async Task Balance() {
            var response = await BaseTest.Client.Balance.Get();

            Assert.AreEqual("EUR", response.Currency);
        }
        
        [Test]
        public async Task Sms()
        {
            var smsParams = new SmsParams("491771783130", "HI2U");

            /*try
            {
                await BaseTest.Client.Sms.Send(smsParams);
                Assert.Pass();
            }
            catch (HttpRequestException e)
            {
                Assert.Fail(e.Message);
            }*/
            
            Assert.DoesNotThrow( () =>
            {
                 _ = BaseTest.Client.Sms.Send(smsParams);
            });
        }
    }
}