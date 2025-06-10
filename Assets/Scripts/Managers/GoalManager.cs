using System;
using System.Collections.Generic;
using GoalRelated;
using Misc;
using ResponsiveUI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GoalManager : MonoBehaviour
    {
        public event Action OnAllGoalsCompleted;
        [SerializeField] private DynamicElementResizer goalContainer;
        private List<GoalConfig> _goalConfigList;
        private readonly Dictionary<GoalType, Goal> _goalDict = new ();
        private int _totalGoalAmount;

        private void OnEnable()
        {
            EventManager.OnGoalProgressed += OnGoalProgressed;
        }

        private void OnDisable()
        {
            EventManager.OnGoalProgressed -= OnGoalProgressed;
        }

        private void OnGoalProgressed(GoalType goalType)
        {
            if (_goalDict.TryGetValue(goalType, out var goal))
            {
                goal.DecreaseGoalCount();
            }
        }

        private void Goal_OnGoalCompleted()
        {
            _totalGoalAmount--;
            if (_totalGoalAmount == 0)
            {
                Debug.Log("All Goals Completed");
                OnAllGoalsCompleted?.Invoke();
            }
        }

        public bool IsAllGoalsCompleted()
        {
            return _totalGoalAmount == 0;
        }

        public void InitializeGoals(List<GoalConfig> goalConfigs)
        {
            _goalConfigList = goalConfigs;
            Setup();
        }
        private void Setup()
        {
            foreach (var goalConfig in _goalConfigList)
            {
                var goal = goalContainer.AddItem(goalConfig.goalPrefab);
                goal.SetGoalCount(goalConfig.goalCount);
                _goalDict.Add(goal.GetGoalType(),goal);
                goal.OnGoalCompleted += Goal_OnGoalCompleted;
                _totalGoalAmount++;
            }

            goalContainer.ResizeElements();
            HorizontalLayoutGroup goalLayoutGroup = goalContainer.GetComponent<HorizontalLayoutGroup>();

            // Force a layout rebuild to ensure proper positioning
            LayoutRebuilder.ForceRebuildLayoutImmediate(goalLayoutGroup.GetComponent<RectTransform>());
            // Make sure when goal elements change size, their position will not be changed by layout group
            goalLayoutGroup.enabled = false;
        }
    }
}