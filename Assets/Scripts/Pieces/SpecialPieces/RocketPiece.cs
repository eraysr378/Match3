using System;
using Cells;
using Interfaces;
using Managers;
using Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pieces
{
    public class RocketPiece : InteractablePiece, IActivatable, ISwappable, IExplodable, ICombinable
    {
        public event Action<IActivatable> OnActivationCompleted;

        public bool IsActivated { get; private set; }

        [SerializeField] private float projectileSpeed;
        [SerializeField] private RocketProjectile projectileLeft;
        [SerializeField] private RocketProjectile projectileRight;
        private bool _isVertical;
        private int _exitedProjectileCount;

        private void OnEnable()
        {
            projectileLeft.OnExitBorders += ProjectileOnExitBorders;
            projectileRight.OnExitBorders += ProjectileOnExitBorders;
        }

        private void OnDisable()
        {
            projectileLeft.OnExitBorders -= ProjectileOnExitBorders;
            projectileRight.OnExitBorders -= ProjectileOnExitBorders;
        }

        public override void Init(Vector3 position)
        {
            base.Init(position);
            SetRandomOrientation();
        }

        public override void Init(Cell cell)
        {
            base.Init(cell);
            _isVertical = Random.Range(0, 2) == 1;
            transform.rotation = Quaternion.Euler(0, 0, _isVertical ? 90 : 0);
        }
        public override void OnSpawn()
        {
            base.OnSpawn();
            projectileLeft.Reset();
            projectileRight.Reset();
            _exitedProjectileCount = 0;
            IsActivated = false;
        }

        private void ProjectileOnExitBorders()
        {
            _exitedProjectileCount++;
            if (_exitedProjectileCount != 2) return;
            SetCell(null);
            OnActivationCompleted?.Invoke(this);
            ReturnToPool();
        }

        public void Explode()
        {
            Activate();
        }

        public void Activate()
        {
            if (IsActivated)
                return;
            EventManager.OnPieceActivated?.Invoke(this);
            IsActivated = true;
            LaunchProjectiles();
        }

        private void LaunchProjectiles()
        {
            transform.SetParent(null);
            projectileLeft.Launch(projectileSpeed, _collider2D);
            projectileRight.Launch(projectileSpeed, _collider2D);
        }


        private void SetRandomOrientation()
        {
            _isVertical = Random.Range(0, 2) == 1;
            transform.rotation = Quaternion.Euler(0, 0, _isVertical ? 90 : 0);
        }

        public void SetHorizontal()
        {
            _isVertical = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        public void SetVertical()
        {
            _isVertical = true;
            transform.rotation = Quaternion.Euler(0, 0, 90);

        }
        public void OnSwap(Piece otherPiece)
        {
            Activate();
        }

        public void OnCombined(Piece piece)
        {
            
        }
    }
}