using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestTimeText = null;
    [SerializeField] private TextMeshProUGUI _currentTimeText = null;

    private bool _isPaused = false;
    private float _currentTime = 0f;
    private float _bestTime = 0f;

    // Getters and Setters
    public float CurrentTime { get => _currentTime; set => _currentTime = value; }

    public float BestTime { get => _bestTime; set => _bestTime = value; }

    private void Awake()
    {
        if (_currentTimeText == null)
        {
            Debug.LogError("Clock: Clock text is null!");
        }

        if (_bestTimeText == null)
        {
            Debug.LogError("Clock: Best time text is null!");
        }

        DisplayText();
    }

    private void Update()
    {
        if (_isPaused) return;

        _currentTime += Time.deltaTime;

        DisplayText();
    }

    private void DisplayText()
    {
        _currentTimeText.text = $"{GetTimeText(_currentTime)}";

        _bestTimeText.text = $"{GetTimeText(_bestTime)}";
    }

    // Get the time in m:ss::mmm format with a bool to display the miliseconds
    public static string GetTimeText(float time, bool displayMiliseconds = false)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int miliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        if (displayMiliseconds)
        {
            return $"{minutes:0}:{seconds:00}:{miliseconds:000}";
        }

        return $"{minutes:0}:{seconds:00}";
    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }
}