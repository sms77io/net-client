#nullable enable
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Hooks;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Hooks {
        [Test]
        public async Task Subscribe_List_Delete()
        {
            var subscribeParams = new SubscribeParams(TestHelper.CreateRandomUrl())
            {
                EventFilter = null,
                EventType = EventType.All,
                RequestMethod = RequestMethod.Json,
            };
            var subscription = await BaseTest.Client.Hooks.Subscribe(subscribeParams);
            Assert.NotNull(subscription.Id);
            
            var response = await BaseTest.Client.Hooks.List();

            Hook? match = null;
            foreach (var entry in response.Entries) {
                if (entry.Id == subscription.Id)
                {
                    match = entry;
                }
                Assert.That(entry.Created, Is.Not.Empty);
                Assert.That(entry.TargetUrl, Is.Not.Empty);
            }
            
            Assert.NotNull(match);
            Assert.AreEqual(subscribeParams.EventFilter, match!.EventFilter);
            Assert.AreEqual(subscribeParams.EventType, match.EventType);
            Assert.AreEqual(subscribeParams.RequestMethod, match.RequestMethod);
            Assert.AreEqual(subscribeParams.TargetUrl, match.TargetUrl);
            
            var unsubscribeParams = new UnsubscribeParams((uint)subscription.Id!);
            var unsubscription = await BaseTest.Client.Hooks.Unsubscribe(unsubscribeParams);
            Assert.True(unsubscription.Success);
        }

        [Test]
        public async Task SubscribeFail() {
            var subscribeParams = new SubscribeParams("")
            {
                EventFilter = null,
                EventType = EventType.All,
                RequestMethod = RequestMethod.Json,
            };
            var subscribed = await BaseTest.Client.Hooks.Subscribe(subscribeParams);

            Assert.AreEqual("Invalid URL", subscribed.ErrorMessage);
            Assert.Null(subscribed.Id);
            Assert.False(subscribed.Success);
        }
    }
}