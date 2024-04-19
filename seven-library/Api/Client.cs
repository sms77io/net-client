using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using seven_library.Api.Library;
using seven_library.Api.Library.Groups;
using seven_library.Api.Library.Hooks;
using seven_library.Api.Library.Journal;
using seven_library.Api.Library.Lookup;
using seven_library.Api.Library.Numbers;
using seven_library.Api.Library.Pricing;
using seven_library.Api.Library.Rcs;
using seven_library.Api.Library.Sms;
using seven_library.Api.Library.Subaccounts;
using seven_library.Api.Library.Voice;

namespace seven_library.Api
{
    public class PagingMetadata
    {
        [JsonProperty("offset")]
        public uint Offset { get; set; }
        
        [JsonProperty("limit")]
        public uint Limit { get; set; }
        
        [JsonProperty("count")]
        public uint Count { get; set; }

        [JsonProperty("total")]
        public uint Total { get; set; }
        
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
    }
    
    public class Client : BaseClient
    {
        public readonly Analytics Analytics;
        public readonly Balance Balance;
        public readonly Contacts Contacts;
        public readonly Groups Groups;
        public readonly Hooks Hooks;
        public readonly Journal Journal;
        public readonly Lookup Lookup;
        public readonly Numbers Numbers;
        public readonly Pricing Pricing;
        public readonly Rcs Rcs;
        public readonly Sms Sms;
        public readonly Subaccounts Subaccounts;
        public readonly Voice Voice;
        public Client(
            string apiKey, 
            string sentWith = "CSharp",
            bool debug = false,
            string? signingSecret = null
        ) : base(apiKey, sentWith, debug, signingSecret)
        {
            Analytics = new Analytics(this);
            Balance = new Balance(this);
            Contacts = new Contacts(this);
            Groups = new Groups(this);
            Hooks = new Hooks(this);
            Journal = new Journal(this);
            Lookup = new Lookup(this);
            Numbers = new Numbers(this);
            Pricing = new Pricing(this);
            Rcs = new Rcs(this);
            Sms = new Sms(this);
            Subaccounts = new Subaccounts(this);
            Voice = new Voice(this);
        }

        private async Task<dynamic> CallDynamicMethod(string name, object?[] paras)
        {
            var methodInfo = GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return await (Task<dynamic>)methodInfo.Invoke(this, paras);
        }
    }
}