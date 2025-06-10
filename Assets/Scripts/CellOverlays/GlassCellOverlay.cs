using System;
using Interfaces;
using Pieces.Behaviors;
using UnityEngine;

namespace CellOverlays
{
    
    public class GlassCellOverlay : BaseCellOverlay,IExplodable,IRainbowHittable,IMatchReactive
    {
        private GoalHandler _goalHandler;

        private void Awake()
        {
            _goalHandler = GetComponent<GoalHandler>();
        }

        public bool TryExplode()
        {
            // Debug.Log("Glass exploded");
            _goalHandler.ReportGoal();
            DestroySelf();
            return true;
        }

        public bool TryHandleRainbowHit(Action onHandled)
        {
            // Debug.Log("Glass hit by rainbow");
            _goalHandler.ReportGoal();
            DestroySelf();
            return true;
        }

        public void OnMatch()
        {
            // Debug.Log("Glass destroyed because match happened");
            _goalHandler.ReportGoal();
            DestroySelf();
        }
    }
}