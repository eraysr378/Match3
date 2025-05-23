using System;
using System.Collections.Generic;
using Cells;
using Combinations;
using Interfaces;
using LevelSystem;
using Pieces;
using Misc;
using ParticleEffects;
using Pieces.Behaviors;
using ScriptableObjects;
using UnityEngine;
using VisualEffects;

namespace Managers
{
    public static class EventManager
    {
        public static Action<SwapHandler,Piece, SwapHandler,Piece> OnSwapRequested;

        public static Func<PieceType, int, int, Piece> OnPieceSpawnRequested;
        public static Func<CellType, Vector3, Cell> OnCellSpawnRequested;
        public static Func<CombinationType, int, int, BaseCombination> OnCombinationSpawnRequested;
        public static Func<int, int, Piece> OnFallingPieceSpawnRequested;
        public static Func<VisualEffectType, BaseVisualEffect> OnVisualEffectSpawnRequested;

        public static Action<Piece> OnPieceReturnToPool;

        public static Action<IActivatable> OnPieceActivated;

        public static Action<Piece, Piece> OnCombinationRequested;
        public static Action<Piece> OnMatchCheckRequested;

        public static Action<BaseCombination> OnCombinationReturnToPool;
        public static Action<BaseVisualEffect> OnVisualEffectReturnToPool;
        public static Action<PoolableParticle> OnParticleReturnToPool;
        public static Func<ParticleType,Transform,Vector3,Vector3, PoolableParticle> OnParticleSpawnRequested;

        public static Action<Piece> OnPieceScored;
        public static Action<List<Piece>> OnMatchHandled;

        public static Action<GoalType> OnGoalProgressed;

        public static Action<LevelDataSo> OnLevelSelected;
    }
}