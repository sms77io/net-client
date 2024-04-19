using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace seven_library.Api.Hooks {
    public class Hooks
    {
        private readonly BaseClient _client;

        public Hooks(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<ListResponse> List()
        {
            var response = await _client.Get("hooks");
            return JsonConvert.DeserializeObject<ListResponse>(response);
        }
        
        public async Task<Subscription> Subscribe(SubscribeParams subscribeParams)
        {
            var response = await _client.Post("hooks", subscribeParams);
            return JsonConvert.DeserializeObject<Subscription>(response);
        }
        
        public async Task<Unsubscription> Unsubscribe(UnsubscribeParams unsubscribeParams)
        {
            var response = await _client.Delete("hooks", unsubscribeParams);
            return JsonConvert.DeserializeObject<Unsubscription>(response);
        }
    }

    public enum Action {
        [EnumMember(Value = "read")]
        Read,
        [EnumMember(Value = "subscribe")]
        Subscribe,
        [EnumMember(Value = "unsubscribe")]
        Unsubscribe
    }
    
    public enum EventType {
        [EnumMember(Value = "all")]
        All,
        [EnumMember(Value = "sms_mo")]
        SmsInbound,
        [EnumMember(Value = "dlr")]
        SmsStatusUpdate,
        [EnumMember(Value = "voice_status")]
        VoiceStatus,
        [EnumMember(Value = "tracking")]
        Tracking,
        [EnumMember(Value = "rcs_dlr")]
        RcsStatusUpdate,
        [EnumMember(Value = "rcs_mo")]
        RcsInbound
    }
    
    public enum RequestMethod {
        [EnumMember(Value = "GET")]
        Get,
        [EnumMember(Value = "JSON")]
        Json,
        [EnumMember(Value = "POST")]
        Post
    }
    
    public class Subscription {
        [JsonProperty("error_message")] 
        public string? ErrorMessage { get; set; }

        [JsonProperty("id")] 
        public uint? Id { get; set; }
        
        [JsonProperty("success")] 
        public bool Success { get; set; }
    }
    
    public class Unsubscription {
        [JsonProperty("success")] public bool Success { get; set; }
    }

    public class ListResponse {
        [JsonProperty("hooks")] 
        public Hook[] Entries { get; set; }
        
        [JsonProperty("success")] 
        public bool Success { get; set; }
    }
    
    public class Hook {
        [JsonProperty("created")]
        public string Created { get; set; }
        
        [JsonProperty("enabled")] 
        public bool Enabled { get; set; }
        
        [JsonProperty("event_filter")] 
        public string? EventFilter { get; set; }
        
        [JsonProperty("event_type")] 
        public EventType EventType { get; set; }
        
        [JsonProperty("id")] 
        public uint Id { get; set; }
                
        [JsonProperty("request_method")] 
        public RequestMethod RequestMethod { get; set; }
        
        [JsonProperty("target_url")] 
        public string TargetUrl { get; set; }
    }
    
    public class SubscribeParams {
        public SubscribeParams(string targetUrl)
        {
            TargetUrl = targetUrl;
        }

        [JsonProperty("event_filter"), JsonConverter(typeof(StringEnumConverter))]
        public string? EventFilter { get; set; }
        
        [JsonProperty("event_type"), JsonConverter(typeof(StringEnumConverter))]
        public EventType EventType { get; set; }
        
        [JsonProperty("request_method"), JsonConverter(typeof(StringEnumConverter))]
        public RequestMethod? RequestMethod { get; set; }
        
        [JsonProperty("target_url")]
        public string TargetUrl { get; set; }
    }
    
    public class UnsubscribeParams {
        public UnsubscribeParams(uint id)
        {
            Id = id;
        }
        
        public UnsubscribeParams(Hook hook)
        {
            Id = hook.Id;
        }
        
        [JsonProperty("id")]
        public uint Id { get; set; }
    }
}