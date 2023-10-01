using UnityEngine;

public class HUDManager : MonoBehaviour
{
    // HUD Elements
    private CollectibleCount _collectibleCount = null;

    private Clock _clock = null;

    private void Awake()
    {
        // The HUD elements are on the layer UI They are disabled by default And are always a child
        // object of the HUDManager Use a function that can find disabled objects
        _collectibleCount = GetComponentInChildren<CollectibleCount>(true);
        if (_collectibleCount == null)
        {
            Debug.LogError("HUDManager: _collectibleCount is null!");
            return;
        }

        _clock = GetComponentInChildren<Clock>(true);
        if (_clock == null)
        {
            Debug.LogError("HUDManager: _clock is null!");
            return;
        }

        // Enable HUD elements by default
        _collectibleCount.gameObject.SetActive(true);
        _clock.gameObject.SetActive(true);
    }

    public void ToggleHud()
    {
        // Check if this object is active
        if (gameObject.activeSelf)
        {
            // If it is, disable it
            gameObject.SetActive(false);
        }
        else
        {
            // If it isn't, enable it
            gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        MenuManager.OnMenuActive += ToggleHud;
    }

    private void OnDisable()
    {
        MenuManager.OnMenuActive -= ToggleHud;
    }
}