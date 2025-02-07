using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace seven_library.Api.Rcs {
    public enum Event {
        [EnumMember(Value = "IS_TYPING")]
        IsTyping,
        [EnumMember(Value = "READ")]
        Read,
    }
    
    public class Resource {
        private readonly BaseClient _client;
        
        public Resource(BaseClient client) {
            _client = client;
        }
        
        public async Task<DispatchResponse> Dispatch(DispatchParams @params) {
            var response = await _client.Post("rcs/messages", @params);

            return JsonConvert.DeserializeObject<DispatchResponse>(response);
        }
        
        public async Task<DeleteResponse> Delete(ulong id) {
            var response = await _client.Delete("rcs/messages/" + id);

            return JsonConvert.DeserializeObject<DeleteResponse>(response);
        }
        
        public async Task<EventResponse> Event(EventParams @params) {
            var response = await _client.Post("rcs/events", @params);

            return JsonConvert.DeserializeObject<EventResponse>(response);
        }
    }
    
    public class Message {
        [JsonProperty("channel")] public string Channel { get; set; }
        [JsonProperty("encoding")] public string Encoding { get; set; }
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("error_text")] public string? ErrorText { get; set; }
        [JsonProperty("fallback")] public string? Fallback { get; set; }
        [JsonProperty("id")] public ulong? Id { get; set; }
        [JsonProperty("is_binary")] public bool IsBinary { get; set; }
        [JsonProperty("parts")] public ushort Parts { get; set; }
        [JsonProperty("price")] public double Price { get; set; }
        [JsonProperty("recipient")] public string Recipient { get; set; }
        [JsonProperty("sender")] public string Sender { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("udh")] public string? UserDataHeader { get; set; }
    }

    public class DeleteResponse {
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class EventResponse {
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class EventParams {
        public EventParams(string to, Event @event, string messageId = "")
        {
            To = to;
            Event = @event;
            MessageId = messageId;
        }

        [JsonProperty("event"), Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))] public Event Event { get; set; }
        [JsonProperty("msg_id")] public string MessageId { get; set; }
        [JsonProperty("to")] public string To { get; set; }
    }
    
    public class DispatchResponse {
        [JsonProperty("success")] public string Success { get; set; }
        [JsonProperty("total_price")] public double TotalPrice { get; set; }
        [JsonProperty("balance")] public double Balance { get; set; }
        [JsonProperty("debug")] public string Debug { get; set; }
        [JsonProperty("sms_type")] public string SmsType { get; set; }
        [JsonProperty("messages")] public Message[] Messages { get; set; }
    }

    public class DispatchParams {
        public DispatchParams(string to, string text)
        {
            To = to;
            Text = text;
        }

        [JsonProperty("to")] public string To { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("from")] public string? From { get; set; }
        [JsonProperty("delay")] public string? Delay { get; set; }
        [JsonProperty("ttl")] public int? Ttl { get; set; }
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("performance_tracking")] public bool? PerformanceTracking { get; set; }
        [JsonProperty("foreign_id")] public string? ForeignId { get; set; }
    }
}