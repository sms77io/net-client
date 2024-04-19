using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Library.Lookup {
    public class Lookup
    {
        private readonly BaseClient _client;

        public Lookup(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<CnamLookup[]> CallerName(LookupParams lookupParams)
        {
            return await Get<CnamLookup>("cnam", lookupParams);
        }
        
        public async Task<FormatLookup[]> Format(LookupParams lookupParams)
        {
            return await Get<FormatLookup>("format", lookupParams);
        }
        
        public async Task<HlrLookup[]> HomeLocationRegister(LookupParams lookupParams)
        {
            return await Get<HlrLookup>("hlr", lookupParams);
        }
        
        public async Task<MnpLookup[]> MobileNumberPortability(LookupParams lookupParams)
        {
            return await Get<MnpLookup>("mnp", lookupParams);
        }
        
        public async Task<RcsCapabilities[]> RcsCapabilities(LookupParams lookupParams)
        {
            return await Get<RcsCapabilities>("rcs", lookupParams);
        }
        
        private async Task<T[]> Get<T>(string type, LookupParams lookupParams)
        {
            var response = await _client.Get($"lookup/{type}", lookupParams);
            return JsonConvert.DeserializeObject<T[]>(response);
        }
    }
    
    public class CnamLookup {
        [JsonProperty("code")] public string Code { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("number")] public string Number { get; set; }
        [JsonProperty("success")] public string Success { get; set; }
    }

    public class FormatLookup {
        [JsonProperty("carrier")] public string Carrier { get; set; }
        [JsonProperty("country_code")] public string CountryCode { get; set; }
        [JsonProperty("country_iso")] public string CountryIso { get; set; }
        [JsonProperty("country_name")] public string CountryName { get; set; }
        [JsonProperty("international")] public string International { get; set; }
        [JsonProperty("international_formatted")] public string InternationalFormatted { get; set; }
        [JsonProperty("national")] public string National { get; set; }
        [JsonProperty("network_type")] public string NetworkType { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class LookupParams {
        public LookupParams(params string[] numbers)
        {
            Numbers = numbers;
        }

        [JsonProperty("number")] 
        public string[] Numbers { get; set; }
    }

    public class Mnp {
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("international_formatted")] public string InternationalFormatted { get; set; }
        [JsonProperty("isPorted")] public bool IsPorted { get; set; }
        [JsonProperty("mccmnc")] public string MobileCountryCodeMobileNetworkCode { get; set; }
        [JsonProperty("national_format")] public string NationalFormat { get; set; }
        [JsonProperty("network")] public string Network { get; set; }
        [JsonProperty("number")] public string Number { get; set; }
    }

    public class Carrier {
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("network_code")] public string NetworkCode { get; set; }
        [JsonProperty("network_type")] public string NetworkType { get; set; }
    }

    public class HlrLookup {
        [JsonProperty("country_code")] public string CountryCode { get; set; }
        [JsonProperty("country_name")] public string CountryName { get; set; }
        [JsonProperty("country_prefix")] public string CountryPrefix { get; set; }
        [JsonProperty("current_carrier")] public Carrier CurrentCarrier { get; set; }
        [JsonProperty("gsm_code")] public string? GsmCode { get; set; }
        [JsonProperty("gsm_message")] public string? GsmMessage { get; set; }
        [JsonProperty("international_format_number")] public string InternationalFormatNumber { get; set; }
        [JsonProperty("international_formatted")] public string InternationalFormatted { get; set; }
        [JsonProperty("lookup_outcome")] public bool LookupOutcome { get; set; }
        [JsonProperty("lookup_outcome_message")] public string LookupOutcomeMessage { get; set; }
        [JsonProperty("national_format_number")] public string NationalFormatNumber { get; set; }
        [JsonProperty("original_carrier")] public Carrier OriginalCarrier { get; set; }
        [JsonProperty("ported")] public string Ported { get; set; }
        [JsonProperty("reachable")] public string Reachable { get; set; }
        [JsonProperty("roaming")] public string Roaming { get; set; }
        [JsonProperty("status")] public bool Status { get; set; }
        [JsonProperty("status_message")] public string StatusMessage { get; set; }
        [JsonProperty("valid_number")] public string ValidNumber { get; set; }
    }

    public class MnpLookup {
        [JsonProperty("code")] public uint Code { get; set; }
        [JsonProperty("mnp")] public Mnp Mnp { get; set; }
        [JsonProperty("price")] public double Price { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class RcsCapabilities : FormatLookup {
        [JsonProperty("rcs_capabilities")] public string[] Capabilities { get; set; }
    }
}