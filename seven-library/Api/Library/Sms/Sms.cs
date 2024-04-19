using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Library.Sms {
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject 
                || reader.TokenType == JsonToken.String
                || reader.TokenType == JsonToken.Integer)
            {
                return new T[] { serializer.Deserialize<T>(reader) };
            }
            return serializer.Deserialize<T[]>(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
    
    public enum StatusCode {
        [EnumMember(Value = "DELIVERED")]
        Delivered,
        [EnumMember(Value = "NOTDELIVERED")]
        NotDelivered,
        [EnumMember(Value = "BUFFERED")]
        Buffered,
        [EnumMember(Value = "TRANSMITTED")]
        Transmitted,
        [EnumMember(Value = "ACCEPTED")]
        Accepted,
        [EnumMember(Value = "EXPIRED")]
        Expired,
        [EnumMember(Value = "REJECTED")]
        Rejected,
        [EnumMember(Value = "FAILED")]
        Failed,
        [EnumMember(Value = "UNKNOWN")]
        Unknown
    }
    
    public class Status {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("status_time")] public string? Time { get; set; }
        [JsonProperty("status")] public StatusCode? Value { get; set; }
    }

    public class StatusParams {
        public StatusParams(params string[] msgIds)
        {
            MessageIds = msgIds;
        }

        [JsonProperty("msg_id")]
        public string[] MessageIds { get; set; }
    }
    
    public class Sms
    {
        private readonly BaseClient _client;

        public Sms(BaseClient client)
        {
            _client = client;
        }
        
        public async Task<Status[]> Status(StatusParams @params)
        {
            var response = await _client.Get("status", @params);
            
            try
            {
                return JsonConvert.DeserializeObject<Status[]>(response);
            }
            catch (JsonException)
            {
                return new Status[] {JsonConvert.DeserializeObject<Status>(response)};
            }
        }
        
        public async Task<DeleteResponse> Delete(DeleteParams deleteParams) {
            var response = await _client.Delete("sms", deleteParams);
            return JsonConvert.DeserializeObject<DeleteResponse>(response);
        }
        
        public async Task<SmsResponse> Send(SmsParams smsParams) {
            var response = await _client.Post("sms", smsParams);
            return JsonConvert.DeserializeObject<SmsResponse>(response);
        }
    }

    public class DeleteParams {
        public DeleteParams(params string[] messageIds)
        {
            MessageIds = messageIds;
        }

        [JsonProperty("msg_ids")] public string[] MessageIds { get; set; }
    }
    
    public class DeleteResponse {
        [JsonProperty("deleted")] public string[] Deleted { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
    }
    
    public class Message {
        [JsonProperty("encoding")] public string Encoding { get; set; }
        [JsonProperty("error")] public string? Error { get; set; }
        [JsonProperty("error_text")] public string? ErrorText { get; set; }
        [JsonProperty("id")] public string? Id { get; set; }
        [JsonProperty("parts")] public ushort Parts { get; set; }
        [JsonProperty("price")] public decimal Price { get; set; }
        [JsonProperty("recipient")] public string Recipient { get; set; }
        [JsonProperty("sender")] public string Sender { get; set; }
        [JsonProperty("success")] public bool Success { get; set; }
        [JsonProperty("text")] public string Text { get; set; }
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