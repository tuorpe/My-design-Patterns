using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace State_Machine
{
    public class GameManagerSM : SingletonMonoBehaviour<GameManagerSM>
    {
        public enum GameState
        {
            MAIN_MENU,
            PLAYING,
            PAUSED, 
            GAME_OVER
        }

        [SerializeField] private GameState gameState;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            AudioManager.Instance.PlayMenuMusic();
            gameState = GameState.MAIN_MENU;
        }

        private void Update()
        {
            switch (gameState)
            {
                case GameState.MAIN_MENU:
                    break;
                case GameState.PLAYING:
                    if(Keyboard.current.pKey.wasPressedThisFrame)
                    {
                        gameState = GameState.PAUSED;
                        Time.timeScale = 0f;
                    }
                    if(Keyboard.current.escapeKey.wasPressedThisFrame)
                    {
                        ReturnToMenu();
                    }
                    break;
                case GameState.PAUSED:
                    if (Keyboard.current.pKey.wasPressedThisFrame)
                    {
                        gameState = GameState.PLAYING;
                        Time.timeScale = 1f;
                    }
                    break;
                case GameState.GAME_OVER:
                
                    break;
            }
        }
        
        public bool isPlaying()
        {
            return gameState == GameState.PLAYING;
        }

        void OnEnable()
        {
            // Rekisteröidään tapahtumakuuntelija
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            // Poistetaan kuuntelija muistivuotojen välttämiseksi
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        public void StartGame()
        {
            AudioManager.Instance?.PlayGameMusic();
            AudioManager.Instance?.PlayButtonClickSound();
            SceneManager.LoadScene("Scenes/State Machine/SMGameScene");
            gameState = GameState.PLAYING;
        }
    
        public void ReturnToMenu()
        {
            AudioManager.Instance?.PlayMenuMusic();
            SceneManager.LoadScene("MenuSceneSM");
            gameState = GameState.MAIN_MENU;
        }
    
        public void OnExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode(); // Exit play mode in the Unity Editor
#else
            Application.Quit();
#endif
        
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "SMGameScene") 
                return;
            var spawner = FindFirstObjectByType<Spawner>();
            if(spawner && AudioManager.Instance)
                spawner.ItemSpawned += AudioManager.Instance.OnItemSpawned;
            gameState = GameState.PLAYING;
        }
    }
}
