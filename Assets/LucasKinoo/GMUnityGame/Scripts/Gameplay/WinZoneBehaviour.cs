using System;
using UnityEngine;

public class WinZoneBehaviour : MonoBehaviour
{
    public static event Action OnWin;

    private bool _hasWon = false;
    private GateBehaviour _winZoneGate = null;

    private void Awake()
    {
        _winZoneGate = GetComponentInChildren<GateBehaviour>();
        if (_winZoneGate == null)
        {
            Debug.LogError("WinZoneBehaviour: _winZoneGate is null!");
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasWon)
        {
            SoundManager._instance.PlaySound(SoundManager._instance.WinSound);
            
            _hasWon = true;
            
            _winZoneGate.OverrideGate(false);

            OnWin?.Invoke();
        }
    }
}