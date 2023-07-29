using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class AnimationsManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<float> _noMatchEvent;
    [SerializeField]
    private UnityEvent<float> _matchEvent;
    [SerializeField]
    private UnityEvent _dropItemsEvent;
    
    [SerializeField]
    private float _endScale;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private float _movementDuration;

    public void ShowSpawnItemAnimation(ItemView itemView)
    {
        itemView.transform.DOScale(_endScale, _scaleDuration);
    }

    public void ShowNoMatchAnimation(ItemView currentItem, ItemView targetItem,
        TweenCallback onAnimationCompleted)
    {
        var swapSequence = GetSwapSequence(currentItem, targetItem);
        swapSequence.SetLoops(2, LoopType.Yoyo);

        swapSequence.OnComplete(onAnimationCompleted);
        _noMatchEvent?.Invoke(_movementDuration);
    }

    public void ShowMatchAnimation(ItemView currentItem, ItemView targetItem, List<Match> matches,
        TweenCallback onAnimationCompleted)
    {
        var firstSequence = GetSwapSequence(currentItem, targetItem);

        var secondSequence = GetMatchSequence(matches, _movementDuration);
        secondSequence.OnComplete(onAnimationCompleted);
        
        firstSequence.Append(secondSequence);
    }

    private Sequence GetSwapSequence(ItemView currentItem, ItemView targetItem)
    {
        var sequence = DOTween.Sequence();
        
        sequence.Join(currentItem.transform.DOMove(targetItem.transform.position, _movementDuration));
        sequence.Join(targetItem.transform.DOMove(currentItem.transform.position, _movementDuration));
        
        return sequence;
    }

    public Sequence GetMatchSequence(IEnumerable<Match> matches, float soundDelay = 0)
    {
        var sequence = DOTween.Sequence();
        
        foreach (var matchItem in matches.SelectMany(match => match.Items))
        {
            sequence.Join(matchItem.transform.DOScale(Vector3.zero, _scaleDuration));
        }
        
        _matchEvent?.Invoke(soundDelay);
        return sequence;
    }

    public void ShowDropItemsAnimation(List<DropItem> dropItems, ItemView[,] items,
        TweenCallback onAnimationCompleted)
    {
        var sequence = DOTween.Sequence();
        
        foreach (var dropItem in dropItems)
        {
            var currentItem = items[dropItem.CurrentCoordinates.x, dropItem.CurrentCoordinates.y];
            var targetItem = items[dropItem.TargetCoordinates.x, dropItem.TargetCoordinates.y];
            sequence.Join(currentItem.transform.DOMove(targetItem.transform.position, _movementDuration));
            sequence.Join(targetItem.transform.DOMove(currentItem.transform.position, _movementDuration));
        }
        
        sequence.OnComplete(onAnimationCompleted);
        _dropItemsEvent?.Invoke();
    }
}