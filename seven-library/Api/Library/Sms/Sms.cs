using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Library.Sms {
    public class Sms
    {
        private readonly BaseClient _client;

        public Sms(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<SmsResponse> Send(SmsParams smsParams) {
            var response = await _client.Post("sms", smsParams);
            return JsonConvert.DeserializeObject<SmsResponse>(response);
        }
    }

    public class Message {
        [JsonProperty("id")] public ulong? Id { get; set; }
        [JsonProperty("sender")] public string Sender { get; set; }
        [JsonProperty("recipient")] public string Recipient { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("encoding")] public string Encoding { get; set; }
        [JsonProperty("parts")] public ushort Parts { get; set; }
        [JsonProperty("price")] public double Price { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("error")] public string Error { get; set; }
        [JsonProperty("error_text")] public string ErrorText { get; set; }
    }

    public class SmsResponse {
        [JsonProperty("success")] public string Success { get; set; }
        [JsonProperty("total_price")] public double TotalPrice { get; set; }
        [JsonProperty("balance")] public double Balance { get; set; }
        [JsonProperty("debug")] public string Debug { get; set; }
        [JsonProperty("sms_type")] public string SmsType { get; set; }
        [JsonProperty("messages")] public Message[] Messages { get; set; }
    }

    public class SmsParams {
        public SmsParams(string to, string text)
        {
            To = to;
            Text = text;
        }

        [JsonProperty("to")] public string To { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("from")] public string? From { get; set; }
        [JsonProperty("delay")] public string? Delay { get; set; }
        [JsonProperty("flash")] public bool? Flash { get; set; }
        [JsonProperty("udh")] public string? UserDataHeader { get; set; }
        [JsonProperty("ttl")] public int? TimeToLive { get; set; }
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("performance_tracking")] public bool? PerformanceTracking { get; set; }
        [JsonProperty("foreign_id")] public string? ForeignId { get; set; }
    }
}