using System.Threading.Tasks;
using Newtonsoft.Json;

namespace seven_library.Api.Groups
{
    public class Resource {
        private readonly BaseClient _client;
        
        public Resource(BaseClient client) {
            _client = client;
        }
        
        public async Task<Group> Create(Group group) {
            var response = await _client.Post("groups", group);

            return JsonConvert.DeserializeObject<Group>(response);
        }
        
        public async Task<Group> One(uint id) {
            var response = await _client.Get($"groups/{id}");

            return JsonConvert.DeserializeObject<Group>(response);
        }
        
        public async Task<GroupsResponse> All() {
            var response = await _client.Get("groups");

            return JsonConvert.DeserializeObject<GroupsResponse>(response);
        }
        
        public async Task<DeleteResponse> Delete(uint id) {
            var response = await _client.Delete($"groups/{id}");

            return JsonConvert.DeserializeObject<DeleteResponse>(response);
        }
    }
    
    public class DeleteResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
    
    public class GroupsResponse
    {
        [JsonProperty("pagingMetadata")]
        public PagingMetadata PagingMetadata { get; set; }
        
        [JsonProperty("data")]
        public Group[] Data { get; set; }
    }
    
    public class Group
    {
        public Group(string name)
        {
            Name = name;
        }

        [JsonProperty("created")]
        public string? Created { get; set; }
        
        [JsonProperty("id")]
        public uint? Id { get; set; }

        [JsonProperty("members_count")]
        public uint? MembersCount { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}