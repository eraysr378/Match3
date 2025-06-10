using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces;
using Projectiles;
using UnityEngine;

namespace Combinations
{
    public abstract class BaseRainbowSpecialPieceCombination<T> : BaseCombination where T : Piece
    {
        [SerializeField] private RainbowProjectile rainbowProjectilePrefab;
        private readonly List<T> _spawnedSpecialPieces = new();
        private int _projectileCount = 0;
        private PieceType _targetType;

        public override void Awake()
        {
            base.Awake();
            _targetType = PieceTypeHelper.GetRandomNormalPieceType();
        }

        protected override void ActivateCombination(int row, int col)
        {
            StartCoroutine(SendProjectiles());
        }

        private IEnumerator SendProjectiles()
        {
            List<Piece> matchingPieces = GridManager.Instance.GetPiecesByType(_targetType)
                .OrderBy(_ => Random.value).ToList();

            _projectileCount = matchingPieces.Count;

            foreach (var piece in matchingPieces)
            {
                if (piece == null)
                    continue;

                SpawnProjectile(piece);
                yield return new WaitForSeconds(0.1f);
            }

            yield break;
        }

        private RainbowProjectile SpawnProjectile(Piece targetPiece)
        {
            var projectile = Instantiate(rainbowProjectilePrefab, transform.position, Quaternion.identity);
            projectile.OnReachedTarget += RainbowProjectileOnReachedTarget;
            projectile.Initialize(targetPiece);
            return projectile;
        }

        private void RainbowProjectileOnReachedTarget(RainbowProjectile projectile, Piece targetPiece)
        {
            projectile.OnReachedTarget -= RainbowProjectileOnReachedTarget;
            if (targetPiece.CurrentCell == null)
            {
                Debug.LogWarning("target piece has no cell");
                _projectileCount--;
                if (_projectileCount == 0)
                {
                    Invoke(nameof(ActivateAllSpecialPieces),1f);
                }

                return;
            }
            T specialPiece = SpawnSpecialPiece(targetPiece.Row, targetPiece.Col);
            (specialPiece as IControlledActivatable)?.WaitForActivation();
            _spawnedSpecialPieces.Add(specialPiece);
            BaseCell baseCell = targetPiece.CurrentCell;
            targetPiece.DestroyPieceInstantly();
            specialPiece.SetCell(baseCell);

            _projectileCount--;
            if (_projectileCount == 0)
            {
                Invoke(nameof(ActivateAllSpecialPieces),1f);
            }
        }

        protected abstract T SpawnSpecialPiece(int row, int col);

        protected virtual void ActivateAllSpecialPieces()
        {
            foreach (var piece in _spawnedSpecialPieces)
            {
                (piece as IControlledActivatable)?.ForceActivate();
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            _targetType = PieceTypeHelper.GetRandomNormalPieceType();
        }
    }
}
