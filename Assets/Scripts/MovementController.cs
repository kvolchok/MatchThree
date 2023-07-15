using DG.Tweening;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private readonly TweenersAwaiter _tweenersAwaiter = new();
    
    private MatchController _matchController;
    private MapIndexProvider _mapIndexProvider;
    private ItemView[,] _items;
    private Camera _camera;
    
    private Vector3 _currentItemPosition;
    private bool _isAnimationPlaying;

    public void Initialize(MatchController matchController, MapIndexProvider mapIndexProvider,
        ItemView[,] items)
    {
        _matchController = matchController;
        _mapIndexProvider = mapIndexProvider;
        _items = items;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isAnimationPlaying)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _currentItemPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }
        
        var movementDirection = GetMovementDirection();
        var currentItemIndex = _mapIndexProvider.GetIndex(_currentItemPosition);

        if (!IsAllowedDirection(movementDirection, currentItemIndex))
        {
            return;
        }
        
        var targetItemIndex = _mapIndexProvider.GetTargetItemIndex(movementDirection, currentItemIndex);

        var currentItem = _items[currentItemIndex.x, currentItemIndex.y];
        var targetItem = _items[targetItemIndex.x, targetItemIndex.y];
        
        var matches = _matchController.GetMatches(currentItemIndex, targetItemIndex, SwapPlaces);
        
        if (matches.Count == 0)
        {
            ShowDoubleSwapAnimation(currentItem, targetItem, OnAnimationCompleted);
        }
        else
        {
            SwapPlaces(currentItemIndex, targetItemIndex);
            ShowSwapAnimation(currentItem, targetItem, OnAnimationCompleted);
        }
    }

    private Vector3 GetMovementDirection()
    {
        var targetItemPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var movementDirection = (targetItemPosition - _currentItemPosition).normalized;
        movementDirection.x = Mathf.RoundToInt(movementDirection.x);
        movementDirection.y = Mathf.RoundToInt(movementDirection.y);

        return movementDirection;
    }

    private bool IsAllowedDirection(Vector3 direction, Vector2Int targetIndex)
    {
        return !IsDiagonalDirection(direction) && !IsOutOfRangeDirection(direction, targetIndex);
    }

    private bool IsDiagonalDirection(Vector3 direction)
    {
        return Mathf.Abs(direction.x) - Mathf.Abs(direction.y) == 0;
    }
    
    private bool IsOutOfRangeDirection(Vector3 direction, Vector2Int targetIndex)
    {
        var isOutOfTopBorder = targetIndex.x - direction.y < 0;
        var isOutOfBottomBorder = targetIndex.x - direction.y >= _items.GetLength(0);
        var isOutOfLeftBorder = targetIndex.y + direction.x < 0;
        var isOutOfRightBorder = targetIndex.y + direction.x >= _items.GetLength(1);

        return isOutOfTopBorder || isOutOfBottomBorder || isOutOfLeftBorder || isOutOfRightBorder;
    }

    private void SwapPlaces(Vector2Int currentIndex, Vector2Int targetIndex)
    {
        (_items[currentIndex.x, currentIndex.y], _items[targetIndex.x, targetIndex.y]) =
            (_items[targetIndex.x, targetIndex.y], _items[currentIndex.x, currentIndex.y]);
    }
    
    private void ShowDoubleSwapAnimation(ItemView currentItem, ItemView targetItem,
        TweenCallback onMovementCompleted)
    {
        _isAnimationPlaying = true;

        var path = new Vector3[2];
        path[0] = targetItem.transform.position;
        path[1] = currentItem.transform.position;
        var currentItemTweener = currentItem.ChangePosition(path);

        path[0] = currentItem.transform.position;
        path[1] = targetItem.transform.position;
        var targetItemTweener = targetItem.ChangePosition(path);
        
        _tweenersAwaiter.Await(onMovementCompleted, currentItemTweener, targetItemTweener);
    }

    private void ShowSwapAnimation(ItemView currentItem, ItemView targetItem, TweenCallback onMovementCompleted)
    {
        _isAnimationPlaying = true;
        
        var currentItemTweener = currentItem.ChangePosition(targetItem.transform.position);
        var targetItemTweener = targetItem.ChangePosition(currentItem.transform.position);
        
        _tweenersAwaiter.Await(onMovementCompleted, currentItemTweener, targetItemTweener);
    }
    
    private void OnAnimationCompleted()
    {
        _isAnimationPlaying = false;
    }
}