using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library.Voice;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Voice {
        private static void AssertResponseObject(VoiceResponse voice) {
            Assert.IsNotEmpty(voice.Success);
        
            foreach (var msg in voice.Messages)
            {
                if (voice.Debug)
                {
                    Assert.IsNull(msg.Id);
                }
            }
        }

        [Test]
        public async Task Text()
        {
            var voiceParams = new VoiceParams("4943130149270", "Hello there!")
            {
                From = "",
                Ringtime = 25,
            };
            var response = await BaseTest.Client.Voice.Call(voiceParams);
            AssertResponseObject(response);
        }

        [Test]
        public async Task Xml()
        {
            var voiceParams = new VoiceParams("4943130149270",
                "<voice name=\"de-de-female\">The total is 13.50 Euros.</voice>");
            var response = await BaseTest.Client.Voice.Call(voiceParams);
            AssertResponseObject(response);
        }
        
        [Test]
        public async Task Validate() {
            var validation = await BaseTest.Client.Voice.Validate(new ValidateParams(TestHelper.PhoneNumber));

            Assert.IsNull(validation.Error);
            Assert.IsNotEmpty(validation.Code);
            Assert.True(validation.Success);
        }

        [Test]
        public async Task ValidateInvalidNumber() {
            const string number = "ThisAintGonnaWork!";
            var validation = await BaseTest.Client.Voice.Validate(new ValidateParams(number));

            Assert.IsNotNull(validation.Error);
            Assert.IsNull(validation.FormattedOutput);
            Assert.IsNull(validation.Id);
            Assert.AreEqual(number, validation.Sender);
            Assert.False(validation.Success);
            Assert.False(validation.Voice);
        }
    }
}