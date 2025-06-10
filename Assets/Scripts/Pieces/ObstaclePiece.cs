using Interfaces;
using UnityEngine;

namespace Pieces
{
    public  class ObstaclePiece : Piece, IExplodable, IAdjacentMatchable
    {
        [SerializeField] protected int maxHealth = 1;
        protected int currentHealth;

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual bool TryExplode()
        {
            TakeDamage();
            return true;
        }

        public virtual void OnAdjacentMatch()
        {
            TakeDamage();
        }

        protected virtual void TakeDamage()
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                Explode();
            }
        }


        protected virtual void Explode()
        {
            Destroy(gameObject);
        }
    }
}