using DG.Tweening;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private readonly TweenersAwaiter _tweenersAwaiter = new();
    
    [SerializeField]
    private float _endScale;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private float _movementDuration;
    
    public void ShowAppearItemAnimation(ItemView itemView)
    {
        itemView.transform.DOScale(_endScale, _scaleDuration);
    }

    public void ShowDoubleSwapAnimation(ItemView currentItem, ItemView targetItem,
        TweenCallback onMovementCompleted)
    {
        var path = new Vector3[2];
        path[0] = targetItem.transform.position;
        path[1] = currentItem.transform.position;
        var currentItemTweener = currentItem.transform.DOPath(path, _movementDuration);

        path[0] = currentItem.transform.position;
        path[1] = targetItem.transform.position;
        var targetItemTweener = targetItem.transform.DOPath(path, _movementDuration);
        
        _tweenersAwaiter.Await(onMovementCompleted, currentItemTweener, targetItemTweener);
    }

    public void ShowSwapAnimation(ItemView currentItem, ItemView targetItem, TweenCallback onMovementCompleted)
    {
        var currentItemTweener = currentItem.transform.DOMove(targetItem.transform.position, _movementDuration);
        var targetItemTweener = targetItem.transform.DOMove(currentItem.transform.position, _movementDuration);

        _tweenersAwaiter.Await(onMovementCompleted, currentItemTweener, targetItemTweener);
    }
}