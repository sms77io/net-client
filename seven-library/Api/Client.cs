using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        public readonly Analytics.Resource Analytics;
        public readonly Balance.Resource Balance;
        public readonly Contacts.Resource Contacts;
        public readonly Groups.Resource Groups;
        public readonly Hooks.Resource Hooks;
        public readonly Journal.Resource Journal;
        public readonly Lookup.Resource Lookup;
        public readonly Numbers.Resource Numbers;
        public readonly Pricing.Resource Pricing;
        public readonly Rcs.Resource Rcs;
        public readonly Sms.Resource Sms;
        public readonly Subaccounts.Resource Subaccounts;
        public readonly Voice.Resource Voice;
        public Client(
            string apiKey, 
            string sentWith = "CSharp",
            bool debug = false,
            string? signingSecret = null
        ) : base(apiKey, sentWith, debug, signingSecret)
        {
            Analytics = new Analytics.Resource(this);
            Balance = new Balance.Resource(this);
            Contacts = new Contacts.Resource(this);
            Groups = new Groups.Resource(this);
            Hooks = new Hooks.Resource(this);
            Journal = new Journal.Resource(this);
            Lookup = new Lookup.Resource(this);
            Numbers = new Numbers.Resource(this);
            Pricing = new Pricing.Resource(this);
            Rcs = new Rcs.Resource(this);
            Sms = new Sms.Resource(this);
            Subaccounts = new Subaccounts.Resource(this);
            Voice = new Voice.Resource(this);
        }

        // ReSharper disable once UnusedMember.Local
        private async Task<dynamic> CallDynamicMethod(string name, object?[] paras)
        {
            var methodInfo = GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return await (Task<dynamic>)methodInfo.Invoke(this, paras);
        }
    }
}