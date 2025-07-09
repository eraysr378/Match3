using System;
using System.Collections.Generic;
using CellOverlays;
using Cells;
using Combinations;
using Interfaces;
using Pieces;
using Misc;
using ParticleEffects;
using Projectiles;
using ScriptableObjects;
using UnityEngine;
using VisualEffects;

namespace Managers
{
    public static class EventManager
    {
        public static Func<PieceType, int, int, Piece> RequestPieceSpawn;
        public static Func<CellType, Vector3, BaseCell> RequestCellSpawn;
        public static Func<CombinationType, int, int, BaseCombination> RequestCombinationSpawn;
        public static Func<int, int, Piece> RequestFallingPieceSpawn;
        public static Func<VisualEffectType, BaseVisualEffect> RequestVisualEffectSpawn;
        public static Func<CellOverlayType, Vector3, BaseCellOverlay> RequestCellOverlaySpawn;
        public static Func<ParticleType,Vector3, PoolableParticle> RequestParticleSpawn;
        public static Func<Vector3,RainbowProjectile> RequestRainbowProjectileSpawn;

        
        public static Action<Piece> ReturnPieceToPool;
        public static Action<BaseCombination> ReturnCombinationToPool;
        public static Action<BaseVisualEffect> ReturnVisualEffectToPool;
        public static Action<PoolableParticle> ReturnParticleToPool;
        public static Action<RainbowProjectile> ReturnRainbowProjectileToPool;
        
        public static Action<IActivatable> OnPieceActivated;
        
        public static Action<Piece, Piece> RequestCombination;
        public static Action<Piece> RequestMatchCheck;
        


        public static Action<Piece> OnPieceScored;
        
        public static Action<GoalType> OnGoalProgressed;

        public static Action<int> OnLevelSelected;
        public static Action OnNextLevelSelected;
        public static Action OnCurrentLevelSelected;

        public static Action OnSmallCameraShakeRequest;
        public static Action OnBigCameraShakeRequest;

    }
}