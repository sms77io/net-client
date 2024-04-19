using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace seven_library.Api.Library
{
    public abstract class BaseClient
    {
        private readonly Dictionary<string, string> _commonPayload = new Dictionary<string, string>();
        protected readonly string ApiKey;
        protected readonly HttpClient Client;
        protected readonly bool Debug;
        protected readonly string SentWith;
        
        public BaseClient(
            string apiKey, 
            string sentWith = "CSharp",
            bool debug = false,
            string? signingSecret = null
            )
        {
            ApiKey = apiKey;
            SentWith = sentWith;
            Debug = debug;

            var httpMessageHandler = new HttpClientHandler();
            httpMessageHandler.Credentials = null;
            var clientOptions = new ClientOptions(apiKey) {
                Debug = debug,
                SentWith = sentWith,
                SigningSecret = signingSecret
            };

            Client = Debug 
                ? new HttpClient(new CustomHttpHandler(new LoggingHandler(httpMessageHandler), clientOptions))
                : new HttpClient(new CustomHttpHandler(httpMessageHandler, clientOptions));
            Client.BaseAddress = new Uri("https://gateway.seven.io/api/");
            
            _commonPayload.Add("p", ApiKey);
            _commonPayload.Add("sentWith", SentWith);
        }

        public async Task<dynamic> Get(string endpoint, object @params = null, NameValueCollection qs = null)
        {
            var query = HttpUtility.ParseQueryString("");
            var requestUri = endpoint;

            if (null != @params)
            {
                foreach (var item in Util.ToJObject(@params))
                {
                    query.Add(item.Key, Util.ToString(item.Value));
                }
            }

            foreach (var item in _commonPayload)
            {
                query.Add(item.Key, item.Value);
            }

            requestUri = $"{endpoint}?{query}";

            if (qs != null) {
                requestUri += $"&{qs}";
            }
                
            return await Client.GetStringAsync(requestUri);
        }

        public async Task<dynamic> Patch(string endpoint, object @params = null)
        {
            var body = new List<KeyValuePair<string, string>>();

            foreach (var item in _commonPayload)
            {
                body.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }

            if (null != @params)
            {
                foreach (var item in Util.ToJObject(@params))
                {
                    body.Add(new KeyValuePair<string, string>(item.Key, Util.ToString(item.Value)));
                }
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);
            request.Content = new FormUrlEncodedContent(body);
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<dynamic> Post(string endpoint, object @params = null)
        {
            var body = new List<KeyValuePair<string, string>>();

            foreach (var item in _commonPayload)
            {
                body.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }

            if (null != @params)
            {
                foreach (var item in Util.ToJObject(@params))
                {
                    body.Add(new KeyValuePair<string, string>(item.Key, Util.ToString(item.Value)));
                }
            }

            var response = await Client.PostAsync(endpoint, new FormUrlEncodedContent(body));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<dynamic> Delete(string endpoint, object @params = null, NameValueCollection qs = null)
        {
            var query = HttpUtility.ParseQueryString("");

            if (null != @params)
            {
                foreach (var item in Util.ToJObject(@params))
                {
                    query.Add(item.Key, Util.ToString(item.Value));
                }
            }

            foreach (var item in _commonPayload)
            {
                query.Add(item.Key, item.Value);
            }

            var requestUri = $"{endpoint}?{query}";

            if (qs != null) {
                requestUri += $"&{qs}";
            }
                
            var task = await Client.DeleteAsync(requestUri);

            return await task.Content.ReadAsStringAsync();
        }
    }
}
