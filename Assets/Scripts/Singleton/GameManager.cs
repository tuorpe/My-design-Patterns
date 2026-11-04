using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused
    }
    public GameState CurrentGameState { get; private set; } = GameState.Menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        AudioManager.Instance.PlayMenuMusic();
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
    public void OnStartGame()
    {
        AudioManager.Instance.PlayGameMusic();
        AudioManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("ObserverScene");
    }
    
    public void OnReturnToMenu()
    {
        AudioManager.Instance.PlayMenuMusic();
        SceneManager.LoadScene("MenuScene");
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
        if (scene.name == "ObserverScene")
        {
            var spawner = FindFirstObjectByType<Spawner>();
            spawner.ItemSpawned += AudioManager.Instance.OnItemSpawned;
        }
    }
}
