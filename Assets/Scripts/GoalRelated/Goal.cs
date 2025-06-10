using System;
using Managers;
using Misc;
using TMPro;
using UnityEngine;

namespace GoalRelated
{
    public class Goal : MonoBehaviour
    {
        public event Action OnGoalCompleted;
        [SerializeField] private TextMeshProUGUI goalText;
        [SerializeField] private GoalType goalType;
        [SerializeField] private GameObject completedTick;
        private int _goalCount;


        public void SetGoalCount(int goalConfigGoalCount)
        {
            UpdateGoalCount(goalConfigGoalCount);
        }

        public void DecreaseGoalCount()
        {
            if (_goalCount <= 0)
                return;
            UpdateGoalCount(_goalCount-1);
            if (_goalCount != 0) 
                return;
            if (goalText == null)
            {
                Debug.LogWarning($"{gameObject.name} something went wrong, goal text is null.");
            }
            goalText.gameObject.SetActive(false);
            completedTick.SetActive(true);
            var starParticle = EventManager.OnParticleSpawnRequested?.Invoke(ParticleType.Stars,transform.position);
            starParticle?.Play();
            OnGoalCompleted?.Invoke();

        }

        private void UpdateGoalCount(int value)
        {
            _goalCount = value;
            goalText.text = _goalCount.ToString();
        }

        public GoalType GetGoalType()
        {
            return goalType;
        }
    }
}
