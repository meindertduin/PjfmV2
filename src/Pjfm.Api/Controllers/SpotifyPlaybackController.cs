using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pjfm.Api.Controllers.Base;

namespace Pjfm.Api.Controllers
{
    [Route("api/playback")]
    public class SpotifyPlaybackController : PjfmController
    {
        public SpotifyPlaybackController(IPjfmControllerContext pjfmContext) : base(pjfmContext)
        {
        }

        [HttpGet("ws")]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(HttpContext, socket);
            }
            else
            {
                HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}