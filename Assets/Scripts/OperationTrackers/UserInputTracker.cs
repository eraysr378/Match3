using Managers;

namespace OperationTrackers
{
    public class GridInputTracker : OperationTracker
    {
        protected override void SubscribeEvents()
        {
            EventManager.OnMatchHandlingStarted += IncreaseActiveOperations;
            EventManager.OnMatchHandlingCompleted += DecreaseActiveOperations;

            EventManager.OnActivationsStarted += IncreaseActiveOperations;
            EventManager.OnActivationsCompleted += DecreaseActiveOperations;

            EventManager.OnFillStarted += IncreaseActiveOperations;
            EventManager.OnFillCompleted += DecreaseActiveOperations;
        }

        protected override void UnsubscribeEvents()
        {
            EventManager.OnMatchHandlingStarted -= IncreaseActiveOperations;
            EventManager.OnMatchHandlingCompleted -= DecreaseActiveOperations;

            EventManager.OnActivationsStarted -= IncreaseActiveOperations;
            EventManager.OnActivationsCompleted -= DecreaseActiveOperations;

            EventManager.OnFillStarted -= IncreaseActiveOperations;
            EventManager.OnFillCompleted -= DecreaseActiveOperations;
        }
    }
}