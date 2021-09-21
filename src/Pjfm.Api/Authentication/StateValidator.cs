using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Pjfm.Common.Extensions;

namespace Pjfm.Api.Authentication
{
    public class StateValidator
    {
        private static ConcurrentDictionary<string, PendingState> PendingStates = new();
        private const int StateExpirationTimeSeconds = 120;
        
        public string GenerateNewState()
        {
            var state = Helpers.RandomString(30);
            PendingStates.TryAdd(state, new PendingState()
            {
                StateSet = DateTime.Now,
            });
            return state;
        }
        
        public bool ValidateState(string state)
        {
            var expirationDate = DateTime.Now.Subtract(TimeSpan.FromSeconds(StateExpirationTimeSeconds));
            
            if (!StateIsValid(state, expirationDate)) return false;

            CleanUpStateAndExpiredStates(state, expirationDate);

            return true;
        }

        private static bool StateIsValid(string state, DateTime expirationDate)
        {
            if (!PendingStates.TryGetValue(state, out var pendingState))
            {
                return false;
            }

            return pendingState.StateSet >= expirationDate;
        }

        private static void CleanUpStateAndExpiredStates(string state, DateTime expirationDate)
        {
            PendingStates.Remove(state, out _);
            
            var expiredDates = PendingStates.Where(x => x.Value.StateSet < expirationDate).ToList();
            foreach (var expiredDate in expiredDates)
            {
                PendingStates.Remove(expiredDate.Key, out _);
            }
        }
    }
    
    internal class PendingState
    {
        public DateTime StateSet { get; set; }
    }
}