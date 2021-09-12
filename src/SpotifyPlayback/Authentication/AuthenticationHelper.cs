using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pjfm.Common.Authentication;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Authentication
{
    public static class AuthenticationHelper
    {
        public static Task HandleUnauthorized(SocketConnection connection, out bool isAuthorized)
        {
            var isAuthenticated = connection.Principal.IsAuthenticated();
            if (isAuthenticated)
            {
                isAuthorized = true;
                return Task.CompletedTask;
            }
            
            isAuthorized = false;
            return SendUnauthorizedResponse(connection);
        }

        public static Task HandleUnauthorized(IEnumerable<UserRole> neededRoles, SocketConnection connection, out bool isAuthorized)
        {
            var hasRequiredRoles = connection.Principal.Roles.All(neededRoles.Contains);
            if (hasRequiredRoles)
            {
                isAuthorized = true;
                return Task.CompletedTask;
            }
            
            isAuthorized = false;
            return SendUnauthorizedResponse(connection);
        }
        private static Task SendUnauthorizedResponse(SocketConnection connection)
        {
            var response = new SocketMessage<object>()
            {
                MessageType = MessageType.Unauthorized,
            };
            return connection.SendMessage(response.GetBytes());
        }
    }
}