using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;

namespace SpotifyPlayback
{
    public class PlaybackWebsocketMiddleware
    {
        private readonly RequestDelegate _next;

        public PlaybackWebsocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/api/playback/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var socket = await context.WebSockets.AcceptWebSocketAsync();
                    var socketDirector = context.RequestServices.GetRequiredService<ISocketDirector>();
                    await socketDirector.HandleSocketConnection(socket, context);
                }
                else
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}