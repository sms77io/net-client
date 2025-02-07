using System;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Contacts;

namespace Seven.Api.Tests
{
    [TestFixture]
    public class Contacts
    {
        [Test]
        public async Task One()
        {
            var initContact = new ContactCreate(groups: [28893])
            {
                Address = "Willestr. 4-6",
                Avatar = "https://www.seven.io/wp-content/uploads/Christian-Leo.png",
                Birthday = "2003-01-01",
                City = "Kiel",
                Email = "tommy.testing@seven.dev",
                Firstname = "Tommy",
                HomeNumber = "49431000000000",
                Lastname = "Testing",
                MobileNumber = "491716992343",
                Notes = "My personal notes",
                PostalCode = "24103",
            };
            var contact = await BaseTest.Client.Contacts.Create(initContact);

            Assert.That(contact.Properties.Address, Is.EqualTo(initContact.Address));
            Assert.That(contact.Avatar, Is.EqualTo(initContact.Avatar));
            Assert.That(contact.Properties.Birthday, Is.EqualTo(initContact.Birthday));
            Assert.That(contact.Properties.City, Is.EqualTo(initContact.City));
            Assert.That(contact.Properties.Email, Is.EqualTo(initContact.Email));
            Assert.That(contact.Properties.Firstname, Is.EqualTo(initContact.Firstname));
            Assert.That(contact.Properties.HomeNumber, Is.EqualTo(initContact.HomeNumber));
            Assert.That(contact.Properties.Lastname, Is.EqualTo(initContact.Lastname));
            Assert.That(contact.Properties.MobileNumber, Is.EqualTo(initContact.MobileNumber));
            Assert.That(contact.Properties.Notes, Is.EqualTo(initContact.Notes));
            Assert.That(contact.Properties.PostalCode, Is.EqualTo(initContact.PostalCode));

            Assert.That(contact.Id, Is.Not.Null);
            
            await BaseTest.Client.Contacts.Delete(contact);
        }

        [Test]
        public async Task All()
        {
            var initContact = new ContactCreate(groups: [])
            {
                Notes = TestHelper.RandomString()
            };
            var createdContact = await BaseTest.Client.Contacts.Create(initContact);

            var response = await BaseTest.Client.Contacts.All();
            var matchedContact = Array.Find(response.Data,
                c => c.Properties.Notes == createdContact.Properties.Notes);
            Assert.That(matchedContact, Is.Not.Null);
            
            await BaseTest.Client.Contacts.Delete(createdContact);
        }

        [Test]
        public async Task Edit()
        {
            var initContact = new ContactCreate(groups: [])
            {
                Firstname = null,
            };
            var contact = await BaseTest.Client.Contacts.Create(initContact);
            contact.Properties.Firstname = "Tommy";

            var updatedContact = await BaseTest.Client.Contacts.Update(contact);
            Assert.That(updatedContact.Properties.Firstname, Is.Not.EqualTo(initContact.Firstname));
        }

        [Test]
        public async Task Delete()
        {
            var contact = await BaseTest.Client.Contacts.Create(new ContactCreate(groups: []));
            await BaseTest.Client.Contacts.Delete(contact);
        }
    }
}