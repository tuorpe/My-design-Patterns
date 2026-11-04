using System;
using System.Collections;
using Observer;
using UnityEngine;
using Object = System.Object;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _coinSound;
    [SerializeField] private AudioClip _enemyHitSound;
    [SerializeField] private AudioClip _obstacleSpawnSound;
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _buttonHoverSound;
    [SerializeField] private AudioClip _buttonClickSound;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    
    [Header("Audio Playback Settings")]
    [SerializeField, Range(0.0f, 1.0f)] private float _sfxVolume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float _musicVolume = 1.0f;
    [SerializeField, Range(0.1f, 2.0f)] private float _sfxPitchMin = 1.0f;
    [SerializeField, Range(0.1f, 2.0f)] private float _sfxPitchMax = 1.0f;
    [SerializeField, Range(0.1f, 5.0f)] private float _audioFadeSpeed = 1.0f;
    
    // Privates 
    private enum FadeState
    {
        In,
        Out,
        End
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Initialize the audio manager
        var itemSpawner = FindFirstObjectByType<Spawner>();
        if(itemSpawner != null)
        {
            itemSpawner.ItemSpawned += OnItemSpawned;
        }
        if(_sfxSource)
        {
            _sfxSource.volume = _sfxVolume;
        }
        if(_musicSource)
        {
            _musicSource.volume = _musicVolume;
            _musicSource.volume = 0f; // Start with music volume at 0 and fade in
        }
    }
    
    #region SoundPlayers

    

    private void PlayCoinSound(Obstacle item)
    {
        if (!_coinSound) 
            return;
        PlayClip(_coinSound);
    }
    
    private void PlayEnemyHitSound(Obstacle item)
    {
        if (!_enemyHitSound) 
            return;
        PlayClip(_enemyHitSound);
    }
    
    public void PlayMenuMusic()
    {
        if (!_menuMusic) 
            return;
        ChangeMusicClip(_menuMusic);
    }
    
    public void PlayGameMusic()
    {
        if (!_gameMusic) 
            return;
        ChangeMusicClip(_gameMusic);
    }

    public void PlayButtonHoverSound()
    {
        if (!_buttonHoverSound) 
            return;
        PlayClip(_buttonHoverSound);
    }
    
    public void PlayButtonClickSound()
    {
        if (!_buttonClickSound) 
            return;
        PlayClip(_buttonClickSound);
    }
    
    public void StopMusic()
    {
        var coroutine = StartMusicChangeCoroutine(null);
        StartCoroutine(coroutine);
    }
    
    private void ChangeMusicClip(AudioClip clip)
    {
        var coroutine = StartMusicChangeCoroutine(clip);
        StartCoroutine(coroutine);
    }

    #endregion
    
    private IEnumerator StartMusicChangeCoroutine(AudioClip clip)
    {
        if (!_musicSource)
        {
            Debug.LogWarning("Music source is not assigned.");
            yield break;
        }
        _musicSource.loop = true;

        var fadeState = FadeState.Out; 
        var musicVolumeTarget = 0f;
        var startVolume = _musicSource.volume;
        var elapsedTime = 0f;
        
        while(true)
        {
            // Move the volume towards the target volume
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / _audioFadeSpeed);
            _musicSource.volume = Mathf.Lerp(startVolume, musicVolumeTarget, progress);
            // State machine to handle fading in and out of music
            switch (fadeState)
            {
                case FadeState.In:
                    musicVolumeTarget = _musicVolume;
                    if (Mathf.Approximately(_musicSource.volume, _musicVolume))
                    {
                        _musicSource.volume = _musicVolume;
                        elapsedTime = 0f;
                        fadeState = FadeState.End;
                    }
                    break;
                case FadeState.Out:
                    musicVolumeTarget = 0f;
                    if (Mathf.Approximately(_musicSource.volume, 0f))
                    {
                        _musicSource.volume = 0f;
                        if (clip)
                        {
                            _musicSource.clip = clip;
                            _musicSource.loop = true;
                            _musicSource.Play();
                            elapsedTime = 0f;
                            fadeState = FadeState.In;
                        }
                        else
                        {
                            _musicSource.Stop();
                            fadeState = FadeState.End;
                        }
                    }
                    break;
            }
            // Exit the loop if the fade state is End
            if(fadeState == FadeState.End)
                break;
            yield return null;
        }
    }

    private void PlayClip(AudioClip clip, float pitch = 0)
    {
        if (!clip) 
            return;
        if (pitch < 0)
            pitch = 0;
        _sfxSource.pitch = pitch > 0.1f ? pitch : UnityEngine.Random.Range(_sfxPitchMin, _sfxPitchMax); // Randomize pitch for variety
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }
    public void OnItemSpawned(Obstacle item)
    {
        // Subscribe to the OnObstacleHit event of the item and play the corresponding sound based on the type of the item
        item.OnObstacleHit += item.type switch
        {
            ObstacleType.Coin => PlayCoinSound,
            ObstacleType.Enemy => PlayEnemyHitSound,
            _ => throw new ArgumentOutOfRangeException()
        };
        _sfxSource.pitch = 1f; // Reset pitch to normal for obstacle spawn sound
        _sfxSource.clip = _obstacleSpawnSound;
        _sfxSource.Play();
    }
}
