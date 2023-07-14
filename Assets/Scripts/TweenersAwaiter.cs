using DG.Tweening;

public class TweenersAwaiter
{
    private Tweener[] _tweeners;
    private TweenCallback _onComplete;

    public void Await(TweenCallback onComplete, params Tweener[] tweeners)
    {
        _onComplete = onComplete;
        _tweeners = tweeners;
        
        foreach (var tweener in _tweeners)
        {
            tweener.OnComplete(OnTweenerComplete);
        }
    }

    private void OnTweenerComplete()
    {
        foreach (var tweener in _tweeners)
        {
            if (tweener.IsPlaying())
            {
                return;
            }
        }
        
        _onComplete?.Invoke();
    }
}