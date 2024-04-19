using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library.Sms;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Sms {
        private const string SuccessCode = "100";

        [Test]
        public async Task Single() {
            var smsParams = new SmsParams(TestHelper.MyPhoneNumber, "HI2U!"){
                Delay = "2050-12-31 00:00:00",
                From = TestHelper.PhoneNumber
            };
            var response = await BaseTest.Client.Sms.Send(smsParams);
            Assert.AreEqual(1, response.Messages.Length);
            var ids = AssertJson(response);

            var deleteResponse = await BaseTest.Client.Sms.Delete(new DeleteParams(ids));
            Assert.True(deleteResponse.Success);
            Assert.AreEqual(ids, deleteResponse.Deleted);
        }

        private static string[] AssertJson(SmsResponse sms) {
            var debug = "true" == sms.Debug;
            double totalPrice = 0;

            var ids = new List<string>();
            foreach (var message in sms.Messages)
            {
                ids.Add(message.Id);
                
                totalPrice += message.Price;

                AssertMessage(message, debug);
            }

            Assert.That(sms.Balance, Is.Positive);
            Assert.That(sms.Debug, Is.EqualTo(debug ? "true" : "false"));
            Assert.That(sms.Messages, Is.Not.Empty);
            Assert.That(sms.Success, Is.EqualTo(SuccessCode));
            StringAssert.IsMatch("direct|economy", sms.SmsType);
            Assert.That(sms.TotalPrice, Is.EqualTo(totalPrice));
            
            return ids.ToArray();
        }

        private static void AssertMessage(Message msg, bool debug) {
            Assert.That(msg.Encoding, Is.Not.Empty);
            Assert.That(msg.Error, Is.Null);
            Assert.That(msg.ErrorText, Is.Null);
            Assert.That(msg.Parts, Is.Positive);
            Assert.That(msg.Recipient, Is.Not.Empty);
            Assert.That(msg.Sender, Is.Not.Empty);
            Assert.That(msg.Success, Is.True);
            Assert.That(msg.Text, Is.Not.Empty);

            if (debug) {
                Assert.That(msg.Id, Is.Null);
                Assert.That(msg.Price, Is.Zero);
            }
            else {
                Assert.That(msg.Id, Is.Positive);
                Assert.That(msg.Price, Is.Positive);
            }
        }
    }
}