using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _instance = null;
    [SerializeField] private AudioSource _effectsSource = null;

    [SerializeField] private AudioClip _alertSound = null;
    [SerializeField] private AudioClip _deathSound = null;
    [SerializeField] private AudioClip _pickupSound = null;
    [SerializeField] private AudioClip _winSound = null;

    // Getters
    public AudioClip AlertSound => _alertSound;
    public AudioClip DeathSound => _deathSound;
    public AudioClip PickupSound => _pickupSound;
    public AudioClip WinSound => _winSound;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Null checks
        if (!_alertSound) Debug.LogError("SoundManager: _alertSound is null!");
        if (!_deathSound) Debug.LogError("SoundManager: _deathSound is null!");
        if (!_pickupSound) Debug.LogError("SoundManager: _pickupSound is null!");
        if (!_winSound) Debug.LogError("SoundManager: _winSound is null!");
    }

    public void PlaySound(AudioClip clip)
    {
        // Make sure a sound is only played if the effects source is not already playing a sound
        _effectsSource.PlayOneShot(clip);
    }
    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
