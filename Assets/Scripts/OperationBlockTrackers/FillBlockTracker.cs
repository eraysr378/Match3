using Managers;
using MatchSystem;
using UnityEngine;

namespace OperationTrackers
{
    public class FillTracker : OperationTracker
    {
        protected override void SubscribeEvents()
        {
            MatchHandler.OnMatchHandlingStarted += IncreaseActiveOperations;
            MatchHandler.OnMatchHandlingCompleted += DecreaseActiveOperations;
            ActivationManager.OnActivationsStarted += IncreaseActiveOperations;
            ActivationManager.OnActivationsCompleted += DecreaseActiveOperations;
            CombinationManager.OnCombinationStarted += IncreaseActiveOperations;
            CombinationManager.OnCombinationCompleted += DecreaseActiveOperations;
        }

        protected override void UnsubscribeEvents()
        {
            MatchHandler.OnMatchHandlingStarted -= IncreaseActiveOperations;
            MatchHandler.OnMatchHandlingCompleted -= DecreaseActiveOperations;
            ActivationManager.OnActivationsStarted -= IncreaseActiveOperations;
            ActivationManager.OnActivationsCompleted -= DecreaseActiveOperations;
            CombinationManager.OnCombinationStarted -= IncreaseActiveOperations;
            CombinationManager.OnCombinationCompleted -= DecreaseActiveOperations;
        }
    }
}