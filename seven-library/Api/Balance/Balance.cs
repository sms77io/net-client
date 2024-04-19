using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Balance
{
    public class Balance {
        private readonly BaseClient _client;
        
        public Balance(BaseClient client) {
            _client = client;
        }
        
        public async Task<BalanceResponse> Get() {
            var response = await _client.Get("balance");
            return JsonConvert.DeserializeObject<BalanceResponse>(response);
        }
    }
    
    public class BalanceResponse
    {
        public BalanceResponse(string currency)
        {
            Currency = currency;
        }

        [JsonProperty("amount")]
        public float Amount { get; set; }
        
        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}