using System.Collections.Generic;
using System.Net.Http;

namespace Pjfm.Common.Http
{
    public class DelegatingRequestMessage : HttpRequestMessage
    {
        public DelegatingRequestMessage(HttpMethod method, string url) : base(method, url)
        {
            
        }
        
        private readonly Dictionary<string, string> _requestMessageParams = new();

        public void SetDelegatingParam(string key, string value)
        {
            _requestMessageParams.Add(key, value);
        }

        public string? GetDelegatingParam(string key)
        {
            _requestMessageParams.TryGetValue(key, out var value);
            return value;
        }
    }
}