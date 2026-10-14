using System;
using UnityEngine;

namespace Observer
{
    public class AchievementController : MonoBehaviour
    {
        public event Action<Achievement> OnAchievementUnlocked;
        private delegate Achievement AchievementDelegate();
        private Achievement _killAchievement;
        private Achievement _coinAchievement;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _killAchievement = new Achievement("Terminator", "Kill them like a terminator. He'll be back", 10);
            _coinAchievement = new Achievement("Coin collector", "Just like Scrooge McDuck", 5);
            FindFirstObjectByType<Spawner>().ItemSpawned += OnItemSpawned;
        }

        private void OnItemSpawned(Obstacle item)
        {
            item.OnObstacleHit += OnHitEvent;
        }

        private void OnDisable()
        {
            // Unsubscribe from all obstacle hit events
            var obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
            foreach (var obstacle in obstacles)
            {
                obstacle.OnObstacleHit -= OnHitEvent;
            }
        }

        public void OnHitEvent(Obstacle obstacle)
        {
            AchievementDelegate achievementDelegate = null;
            switch (obstacle.type)
            {
                case ObstacleType.Enemy:
                    achievementDelegate = _killAchievement.Achieve;
                    break;
                case ObstacleType.Coin:
                    achievementDelegate = _coinAchievement.Achieve;
                    break;
            }
            // Invoke the delegate to check for achievement completion, if the achievement is completed, invoke the OnAchievementUnlocked event
            var achievement = achievementDelegate?.Invoke();
            // If the achievement is completed, invoke the OnAchievementUnlocked event
            if (achievement != null)
            {
                OnAchievementUnlocked?.Invoke(achievement);
            }

        }
    }
}
