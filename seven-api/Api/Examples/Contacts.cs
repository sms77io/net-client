using System;
using System.Threading.Tasks;
using seven_library.Api.Library;

namespace Seven.Api.Examples {
    internal class Contacts : BaseExample {
        public async Task One()
        {
            var response = await Client.Contacts.Create(new Contact());
            var contact = await Client.Contacts.One(response.Id);

            Console.WriteLine(contact);
        }

        public async Task All() {
            var response = await Client.Contacts.All();

            foreach (var contact in response.Data) {
                Console.WriteLine(contact);
            }
        }

        public async Task Create() {
            Console.WriteLine(await Client.Contacts.Create(new Contact()));
        }

        public async Task Edit()
        {
            var contact = await Client.Contacts.Create(new Contact());
            contact.Properties.Firstname = "Tommy";
            
            Console.WriteLine(await Client.Contacts.Update(contact));
        }
        
        public async Task Delete() {
            var contact = await Client.Contacts.Create(new Contact());
            
            await Client.Contacts.Delete(contact.Id);
        }
    }
}