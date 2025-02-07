using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Analytics {
    public class Resource {
        private readonly BaseClient _client;
        
        public Resource(BaseClient client) {
            _client = client;
        }
        
        public async Task<AnalyticsByCountry[]> ByCountry(AnalyticsParams? analyticsParams = null) {
            return await Get<AnalyticsByCountry>("country", analyticsParams);
        }
        
        public async Task<AnalyticsByDate[]> ByDate(AnalyticsParams? analyticsParams = null) {
            return await Get<AnalyticsByDate>("date", analyticsParams);
        }
        
        public async Task<AnalyticsByLabel[]> ByLabel(AnalyticsParams? analyticsParams = null) {
            return await Get<AnalyticsByLabel>("label", analyticsParams);
        }
        
        public async Task<AnalyticsBySubaccount[]> BySubaccount(AnalyticsParams? analyticsParams = null) {
            return await Get<AnalyticsBySubaccount>("subaccount", analyticsParams);
        }
        
        private async Task<T[]> Get<T>(string groupBy, AnalyticsParams? analyticsParams = null)
        {
            analyticsParams ??= new AnalyticsParams();
            var response = await _client.Get("analytics/" + groupBy, analyticsParams);
            return JsonConvert.DeserializeObject<T[]>(response);
        }
    }
    
    public class AnalyticsParams {
        [JsonProperty("end")] 
        public string? End { get; set; }
        
        [JsonProperty("label")] 
        public string? Label { get; set; }
        
        [JsonProperty("start")] 
        public string? Start { get; set; }
        
        [JsonProperty("subaccounts")]
        public string? Subaccounts { get; set; }
    }

    public abstract class AnalyticsBase {
        [JsonProperty("inbound")] 
        public int Inbound { get; set; }
        
        [JsonProperty("hlr")] 
        public int Hlr { get; set; }
        
        [JsonProperty("mnp")] 
        public int Mnp { get; set; }
        
        [JsonProperty("sms")]
        public int Sms { get; set; }
        
        [JsonProperty("usage_eur")]
        public decimal UsageEur { get; set; }
        
        [JsonProperty("voice")]
        public int Voice { get; set; }
    }
    
    public class AnalyticsByCountry : AnalyticsBase {
        [JsonProperty("country")] public string Country { get; set; }
    }
    
    public class AnalyticsByDate : AnalyticsBase {
        [JsonProperty("date")] public string Date { get; set; }
    }
    
    public class AnalyticsByLabel : AnalyticsBase {
        [JsonProperty("label")] public string Label { get; set; }
    }
    
    public class AnalyticsBySubaccount : AnalyticsBase {
        [JsonProperty("account")] public string Account { get; set; }
    }
}