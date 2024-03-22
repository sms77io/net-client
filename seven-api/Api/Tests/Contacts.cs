using System;
using System.Threading.Tasks;
using NUnit.Framework;
using seven_library.Api.Library;

namespace Seven.Api.Tests
{
    [TestFixture]
    public class Contacts
    {
        [Test]
        public async Task One()
        {
            var properties = new Properties
            {
                Firstname = "Tommy"
            };
            var contact = await BaseTest.Client.Contacts.Create(new Contact { Properties = properties });
            var newContact = await BaseTest.Client.Contacts.One(contact);

            Assert.That(newContact.Properties.Firstname, Is.EqualTo(properties.Firstname));

            await BaseTest.Client.Contacts.Delete(contact);
        }

        [Test]
        public async Task All()
        {
            var createdContact = await BaseTest.Client.Contacts.Create(new Contact
            {
                Properties = new Properties
                {
                    Firstname = "Tommy"
                }
            });

            var response = await BaseTest.Client.Contacts.All();
            var matchedContact = Array.Find(response.Data,
                c => c.Properties.Firstname == createdContact.Properties.Firstname);
            Assert.That(matchedContact, Is.Not.Null);
        }

        [Test]
        public async Task Create()
        {
            var contact = await BaseTest.Client.Contacts.Create(new Contact());
            Assert.That(contact.Id, Is.Not.Null);
        }

        [Test]
        public async Task Edit()
        {
            var contact = await BaseTest.Client.Contacts.Create(new Contact());
            contact.Properties.Firstname = "Tommy";

            var newContact = await BaseTest.Client.Contacts.Update(contact);
            Assert.That(newContact.Properties.Firstname, Is.EqualTo(contact.Properties.Firstname));
        }

        [Test]
        public async Task Delete()
        {
            var contact = await BaseTest.Client.Contacts.Create(new Contact());

            await BaseTest.Client.Contacts.Delete(contact);
        }
    }
}