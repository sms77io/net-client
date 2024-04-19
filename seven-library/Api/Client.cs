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
        public readonly Analytics.Analytics Analytics;
        public readonly Balance.Balance Balance;
        public readonly Contacts.Contacts Contacts;
        public readonly Groups.Groups Groups;
        public readonly Hooks.Hooks Hooks;
        public readonly Journal.Journal Journal;
        public readonly Lookup.Lookup Lookup;
        public readonly Numbers.Numbers Numbers;
        public readonly Pricing.Pricing Pricing;
        public readonly Rcs.Rcs Rcs;
        public readonly Sms.Sms Sms;
        public readonly Subaccounts.Subaccounts Subaccounts;
        public readonly Voice.Voice Voice;
        public Client(
            string apiKey, 
            string sentWith = "CSharp",
            bool debug = false,
            string? signingSecret = null
        ) : base(apiKey, sentWith, debug, signingSecret)
        {
            Analytics = new Analytics.Analytics(this);
            Balance = new Balance.Balance(this);
            Contacts = new Contacts.Contacts(this);
            Groups = new Groups.Groups(this);
            Hooks = new Hooks.Hooks(this);
            Journal = new Journal.Journal(this);
            Lookup = new Lookup.Lookup(this);
            Numbers = new Numbers.Numbers(this);
            Pricing = new Pricing.Pricing(this);
            Rcs = new Rcs.Rcs(this);
            Sms = new Sms.Sms(this);
            Subaccounts = new Subaccounts.Subaccounts(this);
            Voice = new Voice.Voice(this);
        }

        // ReSharper disable once UnusedMember.Local
        private async Task<dynamic> CallDynamicMethod(string name, object?[] paras)
        {
            var methodInfo = GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            return await (Task<dynamic>)methodInfo.Invoke(this, paras);
        }
    }
}