using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    public event TweenCallback OnItemsMatched;
    
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
        var firstSequence = DOTween.Sequence();
        var path = new Vector3[2];
        path[0] = targetItem.transform.position;
        path[1] = currentItem.transform.position;
        firstSequence.Append(currentItem.transform.DOPath(path, _movementDuration));

        var secondSequence = DOTween.Sequence();
        path[0] = currentItem.transform.position;
        path[1] = targetItem.transform.position;
        secondSequence.Append(targetItem.transform.DOPath(path, _movementDuration));

        firstSequence.Join(secondSequence);
        firstSequence.Play().OnComplete(onMovementCompleted);
    }

    public void ShowMatchAnimation(ItemView currentItem, ItemView targetItem, List<Match> matches,
        TweenCallback onMovementCompleted)
    {
        var firstSequence = DOTween.Sequence();
        firstSequence.Append(currentItem.transform.DOMove(targetItem.transform.position, _movementDuration));
        var secondSequence = DOTween.Sequence();
        secondSequence.Append(targetItem.transform.DOMove(currentItem.transform.position, _movementDuration));

        firstSequence.Join(secondSequence);
        
        var thirdSequence = GetMatchSequence(matches);

        firstSequence.Append(thirdSequence);
        firstSequence.Play().OnComplete(onMovementCompleted);
    }

    public void ShowDropItemsAnimation(ItemView currentItem, ItemView targetItem)
    {
        currentItem.transform.DOMove(targetItem.transform.position, _movementDuration);
        targetItem.transform.DOMove(currentItem.transform.position, _movementDuration);
    }

    private Sequence GetMatchSequence(List<Match> matches)
    {
        var sequence = DOTween.Sequence();
        
        foreach (var match in matches)
        {
            foreach (var matchItem in match.Items)
            {
                sequence.Join(matchItem.transform.DOScale(Vector3.zero, _scaleDuration));
            }
        }

        sequence.OnComplete(OnItemsMatched);
        return sequence;
    }
}