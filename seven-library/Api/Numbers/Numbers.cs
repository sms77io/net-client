using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using seven_library.Api.Numbers;

namespace seven_library.Api.Numbers
{
    public class Numbers {
        private readonly BaseClient _client;
        
        public Numbers(BaseClient client) {
            _client = client;
        }
        
        public async Task<OrderResponse> Order(OrderParams orderParams) {
            var response = await _client.Post("numbers/order", orderParams);
            return JsonConvert.DeserializeObject<OrderResponse>(response);
        }
        
        public async Task<PhoneNumber> One(OneParams oneParams) {
            var response = await _client.Get($"numbers/active/{oneParams.Number}");
            return JsonConvert.DeserializeObject<PhoneNumber>(response);
        }
        
        public async Task<ListActiveResponse> ListActive() {
            var response = await _client.Get("numbers/active");
            return JsonConvert.DeserializeObject<ListActiveResponse>(response);
        }
        
        public async Task<ListAvailableResponse> ListAvailable(ListAvailableParams availableParams) {
            var response = await _client.Get("numbers/available", availableParams);
            return JsonConvert.DeserializeObject<ListAvailableResponse>(response);
        }
        
        public async Task<DeleteResponse> Delete(DeleteParams deleteParams)
        {
            var endpoint = $"numbers/active/{deleteParams.Number}";
            if (deleteParams.DeleteImmediately)
            {
                endpoint += $"?delete_immediately={deleteParams.DeleteImmediately}";
            }
            return JsonConvert.DeserializeObject<DeleteResponse>(await _client.Delete(endpoint));
        }
    }
    
    public enum PaymentInterval
    {
        [EnumMember(Value = "annually")]
        Annually,
        [EnumMember(Value = "monthly")]
        Monthly,
    }
    
    public class OneParams
    {
        public OneParams(string number)
        {
            Number = number;
        }
        
        public OneParams(PhoneNumberOffer offer)
        {
            Number = offer.Number;
        }

        public string Number { get; set; }
    }
    
    public class DeleteParams
    {
        public DeleteParams(string number)
        {
            Number = number;
        }
        public DeleteParams(PhoneNumber number)
        {
            Number = number.Number;
        }

        public string Number { get; set; }
        public bool DeleteImmediately { get; set; }
    }
    
    public class ListAvailableParams
    {
        [JsonProperty("country")]
        public string? Country { get; set; }
        
        [JsonProperty("features_sms")]
        public bool FeaturesSms { get; set; }
        
        [JsonProperty("features_a2p_sms")]
        public bool FeaturesApplicationToPersonSms { get; set; }
        
        [JsonProperty("features_voice")]
        public bool FeaturesVoice { get; set; }
    }
    
    public class OrderParams
    {
        public OrderParams(string number)
        {
            Number = number;
        }
        public OrderParams(PhoneNumberOffer offer)
        {
            Number = offer.Number;
        }

        [JsonProperty("number")]
        public string Number { get; set; }
        
        [JsonProperty("payment_interval")]
        public PaymentInterval PaymentInterval { get; set; }
    }
    
    public class OrderResponse
    {
        [JsonProperty("error")]
        public string? Error { get; set; }
        
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
    
    public class DeleteResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
    
    public class ListActiveResponse
    {
        public ListActiveResponse(PhoneNumber[] activeNumbers)
        {
            ActiveNumbers = activeNumbers;
        }

        [JsonProperty("activeNumbers")]
        public PhoneNumber[] ActiveNumbers { get; set; }
    }
    
    public class ListAvailableResponse
    {
        public ListAvailableResponse(PhoneNumberOffer[] availableNumbers)
        {
            AvailableNumbers = availableNumbers;
        }

        [JsonProperty("availableNumbers")]
        public PhoneNumberOffer[] AvailableNumbers { get; set; }
    }
    
    public class PhoneNumber
    {
        public PhoneNumber(string country, string number, string friendlyName, string created, PhoneNumberBilling billing, PhoneNumberFeatures features, PhoneNumberForwardInboundSms fowardInboundSms)
        {
            Country = country;
            Number = number;
            FriendlyName = friendlyName;
            Created = created;
            Billing = billing;
            Features = features;
            FowardInboundSms = fowardInboundSms;
        }
            
        [JsonProperty("forward_sms_mo")]
        public PhoneNumberForwardInboundSms FowardInboundSms { get; set; }
        
        [JsonProperty("features")]
        public PhoneNumberFeatures Features { get; set; }
            
        [JsonProperty("billing")]
        public PhoneNumberBilling Billing { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        
        [JsonProperty("number")]
        public string Number { get; set; }
        
        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }
        
        [JsonProperty("expires")]
        public string? Expires { get; set; }
        
        [JsonProperty("created")]
        public string Created { get; set; }
    }
}

public class PhoneNumberBilling
{
    public PhoneNumberBilling(PhoneNumberBillingFees fees)
    {
        Fees = fees;
    }

    [JsonProperty("fees")]
    public PhoneNumberBillingFees Fees { get; set; }
    
    [JsonProperty("payment_interval")]
    public PaymentInterval PaymentInterval { get; set; }
}

public class PhoneNumberBillingFees
{
    [JsonProperty("setup")]
    public float Setup { get; set; }
    
    [JsonProperty("basic_charge")]
    public float BasicCharge { get; set; }
    
    [JsonProperty("sms_mo")]
    public float InboundSms { get; set; }
    
    [JsonProperty("voice_mo")]
    public float InboundVoice { get; set; }
}

public class PhoneNumberFeatures
{
    [JsonProperty("sms")]
    public bool Sms { get; set; }
    
    [JsonProperty("a2p_sms")]
    public bool ApplicationToPersonSms { get; set; }
    
    [JsonProperty("voice")]
    public bool Voice { get; set; }
}

public class PhoneNumberForwardInboundSms
{
    public PhoneNumberForwardInboundSms(PhoneNumberForwardInboundSmsToSms sms, PhoneNumberForwardInboundSmsToMail email)
    {
        Sms = sms;
        Email = email;
    }

    [JsonProperty("sms")]
    public PhoneNumberForwardInboundSmsToSms Sms { get; set; }
    
    [JsonProperty("email")]
    public PhoneNumberForwardInboundSmsToMail Email { get; set; }
}

public class PhoneNumberForwardInboundSmsToSms
{
    public PhoneNumberForwardInboundSmsToSms(string[] numbers)
    {
        Numbers = numbers;
    }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
    
    [JsonProperty("number")]
    public string[] Numbers { get; set; }
}

public class PhoneNumberForwardInboundSmsToMail
{
    public PhoneNumberForwardInboundSmsToMail(string[] addresses)
    {
        Addresses = addresses;
    }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
    
    [JsonProperty("address")]
    public string[] Addresses { get; set; }
}

public class PhoneNumberOffer
{
    public PhoneNumberOffer(string country, string number, string numberParsed, PhoneNumberFeatures features, PhoneNumberOfferFees fees)
    {
        Country = country;
        Number = number;
        NumberParsed = numberParsed;
        Features = features;
        Fees = fees;
    }

    [JsonProperty("country")]
    public string Country { get; set; }
    
    [JsonProperty("number")]
    public string Number { get; set; }
    
    [JsonProperty("number_parsed")]
    public string NumberParsed { get; set; }
    
    [JsonProperty("features")]
    public PhoneNumberFeatures Features { get; set; }
    
    [JsonProperty("fees")]
    public PhoneNumberOfferFees Fees { get; set; }
}

public class PhoneNumberOfferFees
{
    public PhoneNumberOfferFees(PhoneNumberOfferFeesValues annually, float inbondSms, float inboundVoice, PhoneNumberOfferFeesValues monthly)
    {
        Annually = annually;
        InbondSms = inbondSms;
        InboundVoice = inboundVoice;
        Monthly = monthly;
    }

    [JsonProperty("annually")]
    public PhoneNumberOfferFeesValues Annually { get; set; }
    
    [JsonProperty("sms_mo")]
    public float InbondSms { get; set; }
    
    [JsonProperty("voice_mo")]
    public float InboundVoice { get; set; }
    
    [JsonProperty("monthly")]
    public PhoneNumberOfferFeesValues Monthly { get; set; }
}

public class PhoneNumberOfferFeesValues
{
    [JsonProperty("basic_charge")]
    public float BasicCharge { get; set; }
    
    [JsonProperty("setup")]
    public float Setup { get; set; }
}