using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpotifyPlayback.Interfaces;
using SpotifyPlayback.Models;

namespace SpotifyPlayback.Requests
{
    public class PlaybackRequestDispatcher : IPlaybackRequestDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public PlaybackRequestDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task HandlePlaybackSocketRequest<TRequest>(TRequest request, SocketConnection connection) where TRequest : IPlaybackRequest
        {
            var handler = _serviceProvider.GetRequiredService<IPlaybackSocketRequestHandler<TRequest>>();
            return handler.HandleAsync(request, connection);
        }

        public Task HandlePlaybackRequest<TRequest>(TRequest request) where TRequest : IPlaybackRequest
        {
            var handler = _serviceProvider.GetRequiredService<IPlaybackRequestHandler<TRequest>>();
            return handler.HandleAsync(request);
        }

        public Task<TResult> HandlePlaybackRequest<TResult>(IPlaybackRequest<TResult> request)
        {
            var handlerType = typeof(IPlaybackRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));
            
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);
            
            return (Task<TResult>)handler.HandleAsync((dynamic) request);
        }
    }
}