using Managers;
using MatchSystem;

namespace OperationTrackers
{
    public class SwapTracker : OperationTracker
    {
        protected override void SubscribeEvents()
        {
            MatchHandler.OnMatchHandlingStarted += IncreaseActiveOperations;
            MatchHandler.OnMatchHandlingCompleted += DecreaseActiveOperations;
            
            ActivationManager.OnActivationsStarted += IncreaseActiveOperations;
            ActivationManager.OnActivationsCompleted += DecreaseActiveOperations;
            
            FillManager.OnFillStarted += IncreaseActiveOperations;
            FillManager.OnFillCompleted += DecreaseActiveOperations;
        }

        protected override void UnsubscribeEvents()
        {
            MatchHandler.OnMatchHandlingStarted -= IncreaseActiveOperations;
            MatchHandler.OnMatchHandlingCompleted -= DecreaseActiveOperations;
            
            ActivationManager.OnActivationsStarted -= IncreaseActiveOperations;
            ActivationManager.OnActivationsCompleted -= DecreaseActiveOperations;
            
            FillManager.OnFillStarted -= IncreaseActiveOperations;
            FillManager.OnFillCompleted -= DecreaseActiveOperations;
        }
    }
}