public class GameWonMenu : BasicMenu
{
    private void OnEnable()
    {
        _subtitleText = $"COMPLETED IN {Clock.GetTimeText(_clock.CurrentTime)}";
    }
}