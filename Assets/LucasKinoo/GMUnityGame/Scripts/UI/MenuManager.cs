using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Handles opening and closing menus
    private GameOverMenu _gameOverMenu = null;
    private GameWonMenu _gameWonMenu = null;

    // Create an event for when the menu is active
    public static event Action OnMenuActive;

    private void Awake()
    {
        _gameOverMenu = GetComponentInChildren<GameOverMenu>(true);
        if (_gameOverMenu == null)
        {
            Debug.LogError("MenuManager: _gameOverMenu is null!");
            return;
        }

        _gameWonMenu = GetComponentInChildren<GameWonMenu>(true);
        if (_gameWonMenu == null)
        {
            Debug.LogError("MenuManager: _gameWonMenu is null!");
            return;
        }

        // Enable all children so that we can disable them in the editor to avoid clutter
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        // Hide the menus
        _gameOverMenu.Hide();
        _gameWonMenu.Hide();
    }

    public static void ToggleMenu(BasicMenu menu)
    {
        // Check if the menu is active
        if (menu.gameObject.activeSelf)
        {
            // If it is, disable it
            menu.gameObject.SetActive(false);
        }
        else
        {
            // If it isn't, enable it
            menu.gameObject.SetActive(true);
            OnMenuActive?.Invoke();
        }
    }

    private void OnEnable()
    {
        PlayerCharacter.OnPlayerDeath += GameOverMenu;
        WinZoneBehaviour.OnWin += WinMenu;
    }

    private void OnDisable()
    {
        PlayerCharacter.OnPlayerDeath -= GameOverMenu;
        WinZoneBehaviour.OnWin -= WinMenu;
    }

    private void GameOverMenu()
    {
        ToggleMenu(_gameOverMenu);
    }

    private void WinMenu()
    {
        ToggleMenu(_gameWonMenu);
    }
}