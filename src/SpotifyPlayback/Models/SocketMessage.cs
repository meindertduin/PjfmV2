using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpotifyPlayback.Models
{
    public class SocketMessage<T>
    {
        public MessageType MessageType { get; set; }
        public T Body { get; set; }

        public byte[] GetBytes()
        {
            var json = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return Encoding.UTF8.GetBytes(json);
        }
    }
}