public class GameOverMenu : BasicMenu
{
    private void OnEnable()
    {
        _subtitleText = $"BEST: {Clock.GetTimeText(_clock.BestTime)}";
    }
}