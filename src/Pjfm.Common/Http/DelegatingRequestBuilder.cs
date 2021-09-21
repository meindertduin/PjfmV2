using System.Net.Http;

namespace Pjfm.Common.Http
{
    public class DelegatingRequestBuilder
    {
        private DelegatingRequestMessage _requestMessage;
        
        public DelegatingRequestBuilder(HttpMethod method, string url)
        {
            _requestMessage = new DelegatingRequestMessage(method, url);
        }

        public DelegatingRequestBuilder AddRequestParam(string key, string value)
        {
            _requestMessage.SetDelegatingParam(key, value);
            return this;
        }

        public DelegatingRequestMessage Build()
        {
            return _requestMessage;
        }
    }
}