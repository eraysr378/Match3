using System;
using System.Collections.Generic;
using CellOverlays;
using Cells;
using Combinations;
using Interfaces;
using Pieces;
using Misc;
using ParticleEffects;
using ScriptableObjects;
using UnityEngine;
using VisualEffects;

namespace Managers
{
    public static class EventManager
    {
        public static Func<PieceType, int, int, Piece> OnPieceSpawnRequested;
        public static Func<CellType, Vector3, BaseCell> OnCellSpawnRequested;
        public static Func<CombinationType, int, int, BaseCombination> OnCombinationSpawnRequested;
        public static Func<int, int, Piece> OnFallingPieceSpawnRequested;
        public static Func<VisualEffectType, BaseVisualEffect> OnVisualEffectSpawnRequested;
        public static Func<CellOverlayType, Vector3, BaseCellOverlay> OnCellOverlaySpawnRequested;
        public static Func<ParticleType,Vector3, PoolableParticle> OnParticleSpawnRequested;

        public static Action<Piece> OnPieceReturnToPool;

        public static Action<IActivatable> OnPieceActivated;

        public static Action<Piece, Piece> OnCombinationRequested;
        public static Action<Piece> OnMatchCheckRequested;

        public static Action<BaseCombination> OnCombinationReturnToPool;
        public static Action<BaseVisualEffect> OnVisualEffectReturnToPool;
        public static Action<PoolableParticle> OnParticleReturnToPool;

        public static Action<Piece> OnPieceScored;

        public static Action<GoalType> OnGoalProgressed;

        public static Action<LevelDataSo> OnLevelSelected;

        public static Action OnSmallCameraShakeRequest;
        public static Action OnBigCameraShakeRequest;

    }
}