using UnityEngine;

namespace Observer
{
    public class GuiController : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private TMPro.TextMeshProUGUI killsText;
        [SerializeField] private Transform achievementPanel;
        private TMPro.TextMeshProUGUI _achievementNameText;
        private TMPro.TextMeshProUGUI _achievementDescriptionText;

        private int _score;
        private int _kills;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // Initialize the GUI elements
            if(scoreText) scoreText.text = _score.ToString();
            if(killsText) killsText.text = _kills.ToString();
            if(achievementPanel)
            {
                achievementPanel.gameObject.SetActive(false);
                _achievementNameText = achievementPanel.Find("Name").GetComponent<TMPro.TextMeshProUGUI>();
                _achievementDescriptionText = achievementPanel.Find("Description").GetComponent<TMPro.TextMeshProUGUI>();
            }
            FindFirstObjectByType<Spawner>().ItemSpawned += OnItemSpawned;
            FindFirstObjectByType<AchievementController>().OnAchievementUnlocked += OnAchievementUnlocked;
        }

        private void OnItemSpawned(Obstacle item)
        {
            item.OnObstacleHit += OnItemHit;
        }

        private void OnItemHit(Obstacle item)
        {
            // Subscribe to the OnObstacleHit event of the item and update the score or kills based on the type of the item
            switch (item.type)
            {
                case ObstacleType.Coin:
                    _score += item.value;
                    scoreText.text = _score.ToString();
                    break;
                case ObstacleType.Enemy:
                    _kills++;
                    killsText.text = _kills.ToString(); 
                    break;
            }
        }
    
        private void OnAchievementUnlocked(Achievement achievement)
        {
            // Set the achievement text
            if(_achievementNameText != null)
                _achievementNameText.text = achievement.name;
            if(_achievementDescriptionText != null)
                _achievementDescriptionText.text = achievement.description;
            // Show the achievement panel
            achievementPanel.gameObject.SetActive(true);
            // Hide the achievement panel after 3 seconds
            Invoke(nameof(HideAchievementPanel), 3f);
        }
    
        private void HideAchievementPanel()
        {
            if(achievementPanel != null)
                achievementPanel.gameObject.SetActive(false);
        }
    }
}
