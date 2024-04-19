using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace seven_library.Api.Subaccounts {
    public class Subaccounts {
        private readonly BaseClient _client;
        
        public Subaccounts(BaseClient client) {
            _client = client;
        }
        
        public async Task<Subaccount[]> Read() {
            var readParams = new ReadParams();
            var response = await _client.Get("subaccounts", readParams);

            return JsonConvert.DeserializeObject<Subaccount[]>(response);
        }
        
        public async Task<CreateResponse> Create(CreateParams createParams) {
            var response = await _client.Post("subaccounts", createParams);

            return JsonConvert.DeserializeObject<CreateResponse>(response);
        }
        
        public async Task<DeleteResponse> Delete(DeleteParams deleteParams) {
            var response = await _client.Post("subaccounts", deleteParams);

            return JsonConvert.DeserializeObject<DeleteResponse>(response);
        }
        
        public async Task<AutoChargeResponse> AutoCharge(AutoChargeParams autoChargeParams) {
            var response = await _client.Post("subaccounts", autoChargeParams);

            return JsonConvert.DeserializeObject<AutoChargeResponse>(response);
        }
        
        public async Task<TransferCreditsResponse> TransferCredits(TransferCreditsParams transferCreditsParams) {
            var response = await _client.Post("subaccounts", transferCreditsParams);

            return JsonConvert.DeserializeObject<TransferCreditsResponse>(response);
        }
    }
    public enum Action {
        read,
        create,
        delete,
        update,
        transfer_credits
    }
    
    public class Contact {
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
    
    public class AutoTopUp {
        [JsonProperty("amount")] public double? Amount { get; set; }
        [JsonProperty("threshold")] public double? Threshold { get; set; }
    }
    
    public class Subaccount {
        [JsonProperty("auto_topup")] public AutoTopUp AutoTopUp { get; set; }
        [JsonProperty("balance")] public double Balance { get; set; }
        [JsonProperty("company")] public string Company { get; set; }
        [JsonProperty("contact")] public Contact Contact { get; set; }
        [JsonProperty("id")] public int Id { get; set; }
        [JsonProperty("total_usage")] public double TotalUsage { get; set; }
        [JsonProperty("username")] public string? Username { get; set; }
    }
    
    public class CreateResponse {
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("subaccount")] public Subaccount? Subaccount { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class DeleteResponse {
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class TransferCreditsResponse {
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class AutoChargeResponse {
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class Params {
        public Params(Action action) {
            Action = action;
        }
        
        [JsonProperty("action"), JsonConverter(typeof(StringEnumConverter))] public Action Action { get; }
    }
    
    public class ReadParams : Params {
        public ReadParams() : base(Action.read) {}
    }
    
    public class CreateParams : Params {
        public CreateParams(string email, string name) : base(Action.create) {
            Email = email;
            Name = name;
        }

        [JsonProperty("email")] public string Email { get; }
        [JsonProperty("name")] public string Name { get; }
    }
    
    public class DeleteParams : Params {
        public DeleteParams(int id) : base(Action.delete) {
            Id = id;
        }

        [JsonProperty("id")] public int Id { get; }
    }
    
    public class AutoChargeParams : Params {
        public AutoChargeParams(int id, double amount, double threshold) : base(Action.update) {
            Id = id;
            Amount = amount;
            Threshold = threshold;
        }

        [JsonProperty("id")] public int Id { get; }
        [JsonProperty("amount")] public double Amount { get; }
        [JsonProperty("threshold")] public double Threshold { get; }
    }
    
    public class TransferCreditsParams : Params {
        public TransferCreditsParams(int id, double amount) : base(Action.transfer_credits) {
            Id = id;
            Amount = amount;
        }

        [JsonProperty("id")] public int Id { get; }
        [JsonProperty("amount")] public double Amount { get; }
    }
}