using Managers;
using MatchSystem;

namespace OperationBlockTrackers
{
    public class FillBlockTracker : OperationBlockTracker
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