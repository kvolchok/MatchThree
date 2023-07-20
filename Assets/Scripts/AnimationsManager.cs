using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
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
        TweenCallback onAnimationCompleted)
    {
        var sequence = DOTween.Sequence();
        var path = new Vector3[2];
        path[0] = targetItem.transform.position;
        path[1] = currentItem.transform.position;
        sequence.Append(currentItem.transform.DOPath(path, _movementDuration));
        
        path[0] = currentItem.transform.position;
        path[1] = targetItem.transform.position;
        sequence.Join(targetItem.transform.DOPath(path, _movementDuration));
        
        sequence.OnComplete(onAnimationCompleted);
    }

    public void ShowMatchAnimation(ItemView currentItem, ItemView targetItem, List<Match> matches,
        TweenCallback onAnimationCompleted)
    {
        var firstSequence = DOTween.Sequence();
        firstSequence.Append(currentItem.transform.DOMove(targetItem.transform.position, _movementDuration));
        firstSequence.Join(targetItem.transform.DOMove(currentItem.transform.position, _movementDuration));

        var secondSequence = GetMatchSequence(matches, onAnimationCompleted);
        firstSequence.Append(secondSequence);
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
    }

    public Sequence GetMatchSequence(List<Match> matches, TweenCallback onAnimationCompleted)
    {
        var sequence = DOTween.Sequence();
        
        foreach (var match in matches)
        {
            foreach (var matchItem in match.Items)
            {
                sequence.Join(matchItem.transform.DOScale(Vector3.zero, _scaleDuration));
            }
        }

        sequence.OnComplete(onAnimationCompleted);
        return sequence;
    }
}