using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Create a static struct of key names
public static class PrefNames
{
    public static readonly string BestTime = "Best Time";
}

public class GameManager : MonoBehaviour
{
    private Clock _clock = null;
    private int _collectibles = 0;
    
    // PlayerPrefs
    private string _prefBestTime = string.Empty;
    
    public static event Action OnResetProgress;

    //Getter for _collectibles
    public int Collectibles { get => _collectibles; }

    private void Awake()
    {
        _clock = FindObjectOfType<Clock>(true);

        if (_clock == null)
        {
            Debug.LogError("GameManager: _clock is null!");
        }

        LoadHighscores();
    }

    private void OnEnable()
    {
        WinZoneBehaviour.OnWin += SaveHighscores;
        PlayerCharacter.OnPlayerDeath += _clock.PauseTimer;
        Collectible.OnCollectibleCollected += OnCollectibleCollected;
    }

    private void OnDisable()
    {
        WinZoneBehaviour.OnWin -= SaveHighscores;
        PlayerCharacter.OnPlayerDeath += _clock.PauseTimer;
        Collectible.OnCollectibleCollected -= OnCollectibleCollected;
    }

    // Save the highscores
    private void SaveHighscores()
    {
        _clock.PauseTimer();

        // Check if "BestTime" exists
        if (PlayerPrefs.HasKey(_prefBestTime))
        {
            _clock.BestTime = Mathf.Min(_clock.CurrentTime, _clock.BestTime);
        }
        else
        {
            _clock.BestTime = _clock.CurrentTime;
        }

        PlayerPrefs.SetFloat(_prefBestTime, _clock.BestTime);
    }

    private void LoadHighscores()
    {
        // Set the prefs
        _prefBestTime = $"{PrefNames.BestTime} - {SceneManager.GetActiveScene().name}";
        
        _clock.BestTime = PlayerPrefs.GetFloat(_prefBestTime);
    }

    public static string GetPref(string prefName, string levelName)
    {
        string key = $"{PrefNames.BestTime} - {levelName}";
        
        if (PlayerPrefs.HasKey(key))
        {
            return key;
        }
        else
        {
            return string.Empty;
        }
    }    
    public static void RestartGame()
    {
        ResetLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void ResetLevel()
    {
        Collectible.Total = 0;
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        OnResetProgress?.Invoke();
    }

    public static void LoadLevel(string levelName)
    {
        ResetLevel();
        SceneManager.LoadScene(levelName);
    }

    public static void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        ResetLevel();

        // Check if the next scene exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public static void QuitGame()
    {
        Application.Quit();

        // Stop the editor from playing the game
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnCollectibleCollected()
    { ++_collectibles; }
}