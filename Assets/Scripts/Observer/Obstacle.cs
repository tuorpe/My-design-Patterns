using System;
using UnityEngine;

namespace Observer
{
    public enum ObstacleType
    { 
        Coin,
        Enemy
    }
    public class Obstacle: MonoBehaviour
    {
        public event Action<Obstacle> OnObstacleHit;
        public int value;
        public ObstacleType type;

        protected void Hit()
        {
            OnObstacleHit?.Invoke(this);
        }
    }
}