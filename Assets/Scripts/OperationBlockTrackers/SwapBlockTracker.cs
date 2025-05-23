using Managers;
using MatchSystem;

namespace OperationBlockTrackers
{
    public class SwapBlockTracker : OperationBlockTracker
    {
        protected override void SubscribeEvents()
        {
            // MatchHandler.OnMatchHandlingStarted += IncreaseActiveOperations;
            // MatchHandler.OnMatchHandlingCompleted += DecreaseActiveOperations;
            //
            // ActivationManager.OnActivationsStarted += IncreaseActiveOperations;
            // ActivationManager.OnActivationsCompleted += DecreaseActiveOperations;
            //
            // FillManager.OnFillStarted += IncreaseActiveOperations;
            // FillManager.OnAllFillsCompleted += DecreaseActiveOperations;
        }

        protected override void UnsubscribeEvents()
        {
            // MatchHandler.OnMatchHandlingStarted -= IncreaseActiveOperations;
            // MatchHandler.OnMatchHandlingCompleted -= DecreaseActiveOperations;
            //
            // ActivationManager.OnActivationsStarted -= IncreaseActiveOperations;
            // ActivationManager.OnActivationsCompleted -= DecreaseActiveOperations;
            //
            // FillManager.OnFillStarted -= IncreaseActiveOperations;
            // FillManager.OnAllFillsCompleted -= DecreaseActiveOperations;
        }
    }
}