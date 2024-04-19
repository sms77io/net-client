using System.Threading.Tasks;
using NUnit.Framework;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Balance {
        [Test]
        public async Task TestBalance() {
            var response = await BaseTest.Client.Balance.Get();

            Assert.AreEqual("EUR", response.Currency);
        }
    }
}