using System;
using Cells;
using Combinations;
using Interfaces;
using Pieces;
using Misc;
using ParticleEffects;
using UnityEngine;
using VisualEffects;

namespace Managers
{
    public static class EventManager
    {
        public static Action<Piece, Piece> OnSwapRequested;

        public static Func<PieceType, int, int, Piece> OnPieceSpawnRequested;
        public static Func<CellType, int, int, Cell> OnCellSpawnRequested;
        public static Func<CombinationType, int, int, BaseCombination> OnCombinationSpawnRequested;
        public static Func<int, int, Piece> OnRandomNormalPieceSpawnRequested;
        public static Func<VisualEffectType, BaseVisualEffect> OnVisualEffectSpawnRequested;

        public static Action<Piece> OnPieceReturnToPool;

        public static Action<IActivatable> OnPieceActivated;

        public static Action<Piece, Piece> OnCombinationRequested;
        public static Action<Piece> OnMatchCheckRequested;

        public static Action<BaseCombination> OnCombinationReturnToPool;
        public static Action<BaseVisualEffect> OnVisualEffectReturnToPool;
        public static Action<PoolableParticle> OnParticleReturnToPool;
        public static Func<ParticleType,Transform,Vector3,Vector3, PoolableParticle> OnParticleSpawnRequested;
    }
}