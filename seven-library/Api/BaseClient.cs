using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace seven_library.Api
{
    public abstract class BaseClient
    {
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

            var handler = Debug
                ? new CustomHttpHandler(new LoggingHandler(httpMessageHandler), clientOptions)
                : new CustomHttpHandler(httpMessageHandler, clientOptions);
            Client = new HttpClient(handler);
            Client.BaseAddress = new Uri("https://gateway.seven.io/api/");
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Add("SentWith", SentWith);
            Client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
        }

        private static string BuildUrl(string endpoint, object? @params, NameValueCollection? qs)
        {
            var query = HttpUtility.ParseQueryString("");

            if (null != @params)
            {
                foreach (var item in Util.ToJObject(@params))
                {
                    query.Add(item.Key, Util.ToString(item.Value));
                }
            }
            
            if (null != qs)
            {
                foreach (string key in qs)
                {
                    query.Add(key, qs.Get(key));
                }
            }

            return query.Count == 0 ? endpoint : $"{endpoint}?{query}";
        }
        
        public async Task<string> Get(string endpoint, object @params = null, NameValueCollection qs = null)
        {
            return await Client.GetStringAsync(BuildUrl(endpoint, @params, qs));
        }

        public async Task<dynamic> Patch(string endpoint, object @params = null)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint);
            request.Content = BuildBody(@params);
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static FormUrlEncodedContent BuildBody(object @params = null)
        {
            var body = new List<KeyValuePair<string, string>>();

            if (null != @params)
            {
                foreach (var item in Util.ToJObject(@params))
                {
                    if (item.Value is IList)
                    {
                        foreach (var listItem in item.Value)
                        {
                            body.Add(new KeyValuePair<string, string>(item.Key + "[]", listItem.ToString()));
                        }
                    }
                    else
                    {
                        body.Add(new KeyValuePair<string, string>(item.Key, Util.ToString(item.Value)));
                    }
                }
            }

            return new FormUrlEncodedContent(body);
        }
        
        public async Task<dynamic> Post(string endpoint, object @params = null)
        {
            var body = BuildBody(@params);
            var response = await Client.PostAsync(endpoint, body);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<dynamic> Delete(string endpoint, object @params = null, NameValueCollection qs = null)
        {
            var task = await Client.DeleteAsync(BuildUrl(endpoint, @params, qs));
            return await task.Content.ReadAsStringAsync();
        }
    }
}
