using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library.Rcs;
using DeleteResponse = seven_library.Api.Library.Rcs.DeleteResponse;

namespace Seven.Api.Tests
{
    [TestFixture]
    public class Rcs
    {
        private const string SuccessCode = "100";

        [Test]
        public async Task EventIsTyping()
        {
            var @params = new EventParams("4915237035388", Event.IS_TYPING);
            var response = await BaseTest.Client.Rcs.Event(@params);

            AssertEventResponse(response);
        }
        
        [Test]
        public async Task EventRead()
        {
            var @params = new EventParams("4915237035388", Event.READ);
            var response = await BaseTest.Client.Rcs.Event(@params);

            AssertEventResponse(response);
        }
        
        [Test]
        public async Task Delete()
        {
            var dispatchParams = new DispatchParams("491716992343", "Text")
            {
                Delay = "2025-12-12 12:05"
            };
            var json = await BaseTest.Client.Rcs.Dispatch(dispatchParams);
            var msg = json.Messages.First();
            Assert.NotNull(msg.Id);
            var response = await BaseTest.Client.Rcs.Delete((ulong)msg.Id);

            AssertDeleteResponse(response);
        }
        
        [Test]
        public async Task DispatchText()
        {
            var @params = new DispatchParams("491716992343", "Text");
            var json = await BaseTest.Client.Rcs.Dispatch(@params);

            AssertJson(json);
        }

        private static void AssertJson(DispatchResponse rcs)
        {
            var debug = "true" == rcs.Debug;
            double totalPrice = 0;

            foreach (var message in rcs.Messages)
            {
                totalPrice += message.Price;

                Assert.AreEqual( "RCS", message.Channel);
                Assert.That(message.Encoding, Is.Not.Empty);
                Assert.IsNull(message.Error);
                Assert.IsNull(message.ErrorText);
                Assert.That(message.Parts, Is.Positive);
                Assert.That(message.Recipient, Is.Not.Empty);
                Assert.That(message.Sender, Is.Not.Empty);
                Assert.IsTrue(message.Success);
                Assert.That(message.Text, Is.Not.Empty);

                if (debug)
                {
                    Assert.IsNull(message.Id);
                    Assert.That(message.Price, Is.Zero);
                }
                else
                {
                    Assert.That(message.Id, Is.Positive);
                    Assert.That(message.Price, Is.Positive);
                }
            }

            Assert.That(rcs.Balance, Is.Positive);
            Assert.AreEqual(rcs.Debug, debug ? "true" : "false");
            Assert.That(rcs.Messages, Is.Not.Empty);
            Assert.AreEqual(rcs.Success, SuccessCode);
            StringAssert.IsMatch("direct|economy", rcs.SmsType);
            Assert.AreEqual(rcs.TotalPrice, totalPrice);
        }

        private static void AssertEventResponse(EventResponse response)
        {
            Assert.True(response.Success);
        }
        
        private static void AssertDeleteResponse(DeleteResponse response)
        {
            Assert.True(response.Success);
        }
    }
}