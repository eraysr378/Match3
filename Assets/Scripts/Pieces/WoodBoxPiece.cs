using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Pieces.Behaviors;
using Projectiles;
using UnityEngine;

namespace Pieces
{
    public class WoodBoxPiece : Piece,IExplodable,IAdjacentMatchable,IAdjacentExplodable
    {
        [SerializeField] private List<WoodProjectile> woodList;
        private GoalHandler _goalHandler;
        private ScoreHandler _scoreHandler;

        private void Awake()
        {
            _goalHandler = GetComponent<GoalHandler>();
            _scoreHandler = GetComponent<ScoreHandler>();
        }

        public bool TryExplode()
        {
            if (woodList.Count > 0)
            {
                var wood = woodList.First();
                woodList.Remove(wood);
                wood.SplitOff();
            }
            else
            {
                SetCell(null);
                _goalHandler.ReportGoal();
                OnReturnToPool();
            }
            return true;
        }

        public void OnAdjacentMatch()
        {
            TryExplode();
        }

        public void OnAdjacentExplosion()
        {
            TryExplode();
        }
    }
}