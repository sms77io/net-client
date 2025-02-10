using System;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Subaccounts;

namespace Seven.Api.Tests {
    [TestFixture]
    public class Subaccounts {
        [Test]
        public async Task TestSubaccountsCreate() {
            var createParams = new CreateParams("", "");
            var res = await BaseTest.Client.Subaccounts.Create(createParams);

            Assert.That(res, Is.InstanceOf(typeof(CreateResponse)));
            Assert.False(res.Success);
            Assert.NotNull(res.Error);
        }
        
        [Test]
        public async Task TestSubaccountsDelete() {
            var deleteParams = new DeleteParams(0);
            var res = await BaseTest.Client.Subaccounts.Delete(deleteParams);

            Assert.That(res, Is.InstanceOf(typeof(DeleteResponse)));
            Assert.False(res.Success);
            Assert.NotNull(res.Error);
        }
        
        [Test]
        public async Task TestSubaccountsRead() {
            var createParams = new CreateParams(TestHelper.RandomString() + "@seven.dev", "Tommy Tester");
            var res = await BaseTest.Client.Subaccounts.Create(createParams);
            
            var subaccounts = await BaseTest.Client.Subaccounts.Read();

            foreach (var subaccount in subaccounts) {
                Assert.That(subaccount, Is.InstanceOf(typeof(Subaccount)));
            }
            
            var deleteParams = new DeleteParams(res.Subaccount.Id);
            await BaseTest.Client.Subaccounts.Delete(deleteParams);
        }
        
        [Test]
        public async Task TestSubaccountsAutoCharge() {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var createParams = new CreateParams($"net_{timestamp}@seven.dev", "Tommy Tester");
            var createResponse = await BaseTest.Client.Subaccounts.Create(createParams);
            var subaccount = createResponse.Subaccount;
            
            var autoChargeParams = new AutoChargeParams(subaccount.Id, 0.0, 0.0);
            var autoChargeResponse = await BaseTest.Client.Subaccounts.AutoCharge(autoChargeParams);

            Assert.True(autoChargeResponse.Success);
            
            var deleteParams = new DeleteParams(subaccount.Id);
            await BaseTest.Client.Subaccounts.Delete(deleteParams);
        }
        
        [Test]
        public async Task TestSubaccountsTransferCredits() {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var createParams = new CreateParams($"net_{timestamp}@seven.dev", "Tommy Tester");
            var createResponse = await BaseTest.Client.Subaccounts.Create(createParams);
            var subaccount = createResponse.Subaccount;
            
            var transferCreditsParams = new TransferCreditsParams(subaccount.Id, 1.0);
            var transferCreditsResponse = await BaseTest.Client.Subaccounts.TransferCredits(transferCreditsParams);

            Assert.True(transferCreditsResponse.Success);
            
            var deleteParams = new DeleteParams(subaccount.Id);
            await BaseTest.Client.Subaccounts.Delete(deleteParams);
        }
    }
}