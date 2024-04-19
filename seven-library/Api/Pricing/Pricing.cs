using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Pricing {
    public class Resource
    {
        private readonly BaseClient _client;

        public Resource(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<PricingResponse> Get(PricingParams? pricingParams = null) {
            pricingParams ??= new PricingParams();
            var response = await _client.Get("pricing", pricingParams);
            return JsonConvert.DeserializeObject<PricingResponse>(response);
        }
    }

    public class Country {
        [JsonProperty("countryCode")] public string CountryCode { get; set; }
        [JsonProperty("countryName")] public string CountryName { get; set; }
        [JsonProperty("countryPrefix")] public string CountryPrefix { get; set; }
        [JsonProperty("networks")] public List<Network> Networks { get; set; }
    }

    public class PricingResponse {
        [JsonProperty("countCountries")] public int CountCountries { get; set; }
        [JsonProperty("countNetworks")] public int CountNetworks { get; set; }
        [JsonProperty("countries")] public List<Country> Countries { get; set; }
    }

    public class Network {
        [JsonProperty("comment")] public string Comment { get; set; }
        [JsonProperty("features")] public List<string> Features { get; set; }
        [JsonProperty("mcc")] public string MobileCountryCode { get; set; }
        [JsonProperty("mncs")] public List<string> MobileNetworkCodes { get; set; }
        [JsonProperty("networkName")] public string NetworkName { get; set; }
        [JsonProperty("price")] public double Price { get; set; }
    }

    public class PricingParams {
        [JsonProperty("country")] public string Country { get; set; }
    }
}