using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Journal {
    public class Journal
    {
        private readonly BaseClient _client;

        public Journal(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<JournalBase[]> Inbound(JournalParams? journalParams = null) {
            return await Get<JournalBase>("inbound", journalParams);
        }
        
        public async Task<JournalOutbound[]> Outbound(JournalParams? journalParams = null) {
            return await Get<JournalOutbound>("outbound", journalParams);
        }
        
        public async Task<JournalBase[]> Replies(JournalParams? journalParams = null) {
            return await Get<JournalBase>("replies", journalParams);
        }
        
        public async Task<JournalVoice[]> Voice(JournalParams? journalParams = null) {
            return await Get<JournalVoice>("voice", journalParams);
        }
        
        private async Task<T[]> Get<T>(string type, JournalParams? journalParams = null)
        {
            journalParams ??= new JournalParams();
            var response = await _client.Get($"journal/{type}", journalParams);
            return JsonConvert.DeserializeObject<T[]>(response);
        }
    }

    public class JournalParams {
        [JsonProperty("date_from")] public string? DateFrom { get; set; }
        [JsonProperty("date_to")] public string? DateTo { get; set; }
        [JsonProperty("id")] public uint? Id { get; set; }
        [JsonProperty("limit")] public uint? Limit { get; set; }
        [JsonProperty("offset")] public uint? Offset { get; set; }
        [JsonProperty("state")] public string? State { get; set; }
        [JsonProperty("to")] public string? To { get; set; }
    }

    public class JournalBase {
        [JsonProperty("from")] public string From { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("to")] public string To { get; set; }
        [JsonProperty("price")] public string Price { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("timestamp")] public string Timestamp { get; set; }
    }

    public class JournalOutbound : JournalBase {
        [JsonProperty("connection")] public string Connection { get; set; }
        [JsonProperty("dlr")] public string? Dlr { get; set; }
        [JsonProperty("dlr_timestamp")] public string? DlrTimestamp { get; set; }
        [JsonProperty("foreign_id")] public string? ForeignId { get; set; }
        [JsonProperty("label")] public string? Label { get; set; }
        [JsonProperty("latency")] public string? Latency { get; set; }
        [JsonProperty("mccmnc")] public string? MccMnc { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
    }
    
    public class JournalVoice : JournalBase {
        [JsonProperty("duration")] public string Duration { get; set; }
        [JsonProperty("error")] public string Error { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("xml")] public bool Xml { get; set; }
    }
}