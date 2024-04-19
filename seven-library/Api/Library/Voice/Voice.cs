using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Library.Voice{
    public class Voice
    {
        private readonly BaseClient _client;

        public Voice(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<VoiceResponse> Call(VoiceParams @params)
        {
            var response = await _client.Post("voice", @params);
            return JsonConvert.DeserializeObject<VoiceResponse>(response);
        }
        
        public async Task<ValidateResponse> Validate(ValidateParams @params)
        {
            var response = await _client.Post("validate_for_voice", @params);
            return JsonConvert.DeserializeObject<ValidateResponse>(response);
        }
    }
    
    public class ValidateResponse {
        [JsonProperty("code")] public string? Code { get; set; }
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("formatted_output")] public string? FormattedOutput { get; set; }
        [JsonProperty("id")] public long? Id { get; set; }
        [JsonProperty("sender")] public string?Sender { get; set; }
        [JsonProperty("success")] public bool? Success { get; set; }
        [JsonProperty("voice")] public bool? Voice { get; set; }
    }

    public class ValidateParams {
        public ValidateParams(string number)
        {
            Number = number;
        }

        [JsonProperty("callback")] public string? Callback { get; set; }
        [JsonProperty("number")] public string Number { get; set; }
    }

    public class VoiceResponse {
        [JsonProperty("balance")] public decimal Balance { get; set; }
        [JsonProperty("debug")] public bool Debug { get; set; }
        [JsonProperty("messages")] public VoiceMessage[] Messages { get; set; }
        [JsonProperty("success")] public string Success { get; set; }
        [JsonProperty("total_price")] public decimal TotalPrice { get; set; }
    }
    
    public class VoiceMessage {
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("error_text")] public string? ErrorText { get; set; }
        [JsonProperty("id")] public uint? Id { get; set; }
        [JsonProperty("price")] public decimal Price { get; set; }
        [JsonProperty("recipient")] public string Recipient { get; set; }
        [JsonProperty("sender")] public string Sender { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
    }

    public class VoiceParams {
        public VoiceParams(string to, string text)
        {
            Text = text;
            To = to;
        }

        [JsonProperty("from")] public string? From { get; set; }
        [JsonProperty("ringtime")] public uint? Ringtime { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("to")] public string To { get; set; }
    }
}