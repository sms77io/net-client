using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Groups;

namespace Seven.Api.Tests
{
    [TestFixture]
    public class Groups
    {
        [Test]
        public async Task All()
        {
            var @params = new Group("C# Group #2");
            var created = await BaseTest.Client.Groups.Create(@params);
            var response = await BaseTest.Client.Groups.All();

            Assert.That(response.PagingMetadata.Offset, Is.EqualTo(0));
            //Assert.That(response.PagingMetadata.Limit, Is.EqualTo(0));
            Assert.That(response.Data, Is.Not.Empty);

            foreach (var group in response.Data)
            {
                Assert.That(group.Created, Is.Not.Empty);
                Assert.That(group.Id, Is.GreaterThan(0));
                Assert.That(group.MembersCount, Is.Not.Negative);
                Assert.That(group.Name, Is.Not.Empty);
            }
            
            await BaseTest.Client.Groups.Delete((uint)created.Id);
        }
        
        [Test]
        public async Task Create()
        {
            var @params = new Group("C# Group #2");
            var group = await BaseTest.Client.Groups.Create(@params);

            Assert.That(group.Created, Is.Not.Empty);
            Assert.That(group.Id, Is.GreaterThan(0));
            Assert.That(group.MembersCount, Is.EqualTo(0));
            Assert.That(group.Name, Is.EqualTo(@params.Name));
            
            await BaseTest.Client.Groups.Delete((uint)group.Id);
        }

        [Test]
        public async Task One()
        {
            var created = await BaseTest.Client.Groups.Create(new Group("C# Group"));
            Assert.That(created.Id, Is.Not.Null);
            var group = await BaseTest.Client.Groups.One((uint)created.Id);

            Assert.That(group.Created, Is.Not.Empty);
            Assert.That(group.Id, Is.GreaterThan(0));
            Assert.That(group.MembersCount, Is.Not.Negative);
            Assert.That(group.Name, Is.Not.Empty);
            
            await BaseTest.Client.Groups.Delete((uint)group.Id);
        }
        
        [Test]
        public async Task Delete()
        {
            var created = await BaseTest.Client.Groups.Create(new Group("C# Group"));
            Assert.That(created.Id, Is.Not.Null);
            var response = await BaseTest.Client.Groups.Delete((uint)created.Id);

            Assert.That(response.Success, Is.True);
        }
    }
}