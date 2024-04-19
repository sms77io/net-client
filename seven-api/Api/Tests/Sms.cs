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
            
            var statuses = await BaseTest.Client.Sms.Status(new StatusParams(ids));
            Assert.AreEqual(ids.Length, statuses.Length);
            var status = statuses.First();
            Assert.AreEqual(ids.First(), status.Id);
            Assert.Null(status.Time);
            Assert.Null(status.Value);
            //StringAssert.IsMatch(string.Join("|", Enum.GetNames(typeof(StatusCode))), status.Value.ToString());
            //Assert.True(Util.IsValidDate(status.Time, "yyyy-MM-dd HH:mm:ss.fff"));

            var deleteResponse = await BaseTest.Client.Sms.Delete(new DeleteParams(ids));
            Assert.True(deleteResponse.Success);
            Assert.AreEqual(ids, deleteResponse.Deleted);
        }

        private static string[] AssertJson(SmsResponse sms) {
            var debug = "true" == sms.Debug;
            var totalPrice = (decimal)0.12;

            var ids = new List<string>();
            foreach (var message in sms.Messages)
            {
                ids.Add(message.Id);
                
                totalPrice += message.Price;

                AssertMessage(message, debug);
            }

            Assert.That(sms.Balance, Is.Positive);
            Assert.AreEqual(debug ? "true" : "false", sms.Debug);
            Assert.IsNotEmpty(sms.Messages);
            Assert.AreEqual(SuccessCode, sms.Success);
            StringAssert.IsMatch("direct|economy", sms.SmsType);
            Assert.AreEqual(totalPrice, sms.TotalPrice);
            
            return ids.ToArray();
        }

        private static void AssertMessage(Message msg, bool debug) {
            Assert.IsNotEmpty(msg.Encoding);
            Assert.IsNull(msg.Error);
            Assert.IsNotNull(msg.ErrorText);
            Assert.That(msg.Parts, Is.Positive);
            Assert.IsNotEmpty(msg.Recipient);
            Assert.IsNotEmpty(msg.Sender);
            Assert.True(msg.Success);
            Assert.IsNotEmpty(msg.Text);

            if (debug) {
                Assert.IsNull(msg.Id);
                Assert.That(msg.Price, Is.Zero);
            }
            else {
                Assert.That(msg.Id, Is.Positive);
                Assert.That(msg.Price, Is.Positive);
            }
        }
    }
}