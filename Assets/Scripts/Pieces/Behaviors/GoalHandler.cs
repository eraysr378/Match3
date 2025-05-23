using Managers;
using Misc;
using UnityEngine;

namespace Pieces.Behaviors
{
  
    public class GoalHandler : MonoBehaviour
    {
        [SerializeField] private GoalType goalType;

        public void ReportGoal()
        {
            EventManager.OnGoalProgressed.Invoke(goalType);
        }
    }
}