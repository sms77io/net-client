#nullable enable
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Journal;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Journal {
        private static void AssertBase(JournalBase entry)
        {
            Assert.That(entry.From, Is.Not.Empty);
            Assert.That(entry.Id, Is.Not.Empty);
            Assert.That(entry.Price, Is.Not.Empty);
            Assert.That(entry.Text, Is.Not.Empty);
            Assert.That(entry.Timestamp, Is.Not.Empty);
            Assert.That(entry.To, Is.Not.Empty);
        }
        
        [Test]
        public async Task Inbound() {
            var list = await BaseTest.Client.Journal.Inbound();
            
            foreach (var entry in list)
            {
                AssertBase(entry);
            }
        }
        
        [Test]
        public async Task Outbound() {
            var list = await BaseTest.Client.Journal.Outbound();

            foreach (var item in list)
            {
                AssertBase(item);
                
                Assert.That(item.Connection, Is.Not.Empty);
                Assert.That(item.Type, Is.Not.Empty);
            }
        }
        
        [Test]
        public async Task Replies() {
            var list = await BaseTest.Client.Journal.Replies();

            foreach (var item in list)
            {
                AssertBase(item);
            }
        }
        
        [Test]
        public async Task Voice() {
            var list = await BaseTest.Client.Journal.Voice(new JournalParams
            {
                DateFrom = null,
                DateTo = null,
                Id = null,
                Limit = null,
                Offset = null,
                State = null,
                To = null
            });

            foreach (var item in list)
            {
                AssertBase(item);
                
                Assert.That(item.Duration, Is.Not.Empty);
                Assert.That(item, Has.Property("Error"));
                Assert.That(item.Status, Is.Not.Empty);
                Assert.That(item.Text, Is.Not.Empty);
                Assert.That(item, Has.Property("Xml"));
            }
        }
    }
}