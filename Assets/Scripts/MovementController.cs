using DG.Tweening;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private readonly TweenersAwaiter _tweenersAwaiter = new();
    
    private MatchThreeController _matchThreeController;
    private MapIndexProvider _mapIndexProvider;
    private ItemView[,] _items;
    private Camera _camera;
    
    private Vector3 _currentItemPosition;
    private Vector3 _movementDirection;
    private Vector2Int _currentItemIndex;
    private Vector2Int _targetItemIndex;

    private bool _isMoving;

    public void Initialize(MatchThreeController matchThreeController, MapIndexProvider mapIndexProvider,
        ItemView[,] items)
    {
        _matchThreeController = matchThreeController;
        _mapIndexProvider = mapIndexProvider;
        _items = items;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isMoving)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _currentItemPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (Input.GetMouseButton(0))
        {
            var targetItemPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _movementDirection = (targetItemPosition - _currentItemPosition).normalized;

            _movementDirection.x = Mathf.RoundToInt(_movementDirection.x);
            _movementDirection.y = Mathf.RoundToInt(_movementDirection.y);
        }

        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }
        
        _currentItemIndex = _mapIndexProvider.GetIndex(_currentItemPosition);

        if (!IsAllowedDirection(_movementDirection, _currentItemIndex))
        {
            return;
        }
        
        _targetItemIndex = _mapIndexProvider.GetTargetItemIndex(_currentItemIndex, _movementDirection);
        
        SwapPlaces(_currentItemIndex, _targetItemIndex);
        
        var currentItem = _items[_currentItemIndex.x, _currentItemIndex.y];
        var targetItem = _items[_targetItemIndex.x, _targetItemIndex.y];

        _isMoving = true;
        
        if (!_matchThreeController.IsMatchThreeByIndex(_currentItemIndex) &&
            !_matchThreeController.IsMatchThreeByIndex(_targetItemIndex))
        {
            SwapPlaces(_currentItemIndex, _targetItemIndex);
            ShowDoubleSwapAnimation(currentItem, targetItem, OnStopMoving);
        }
        else
        {
            ShowSwapAnimation(currentItem, targetItem, OnStopMoving);
        }
    }

    private void OnStopMoving()
    {
        _isMoving = false;
    }
    
    private bool IsAllowedDirection(Vector3 direction, Vector2Int index)
    {
        if (Mathf.Abs(direction.x) - Mathf.Abs(direction.y) == 0)
        {
            return false;
        }
        
        if (index.x - direction.y < 0 || index.x - direction.y >= _items.GetLength(0) ||
            index.y + direction.x < 0 || index.y + direction.x >= _items.GetLength(1))
        {
            return false;
        }

        return true;
    }

    private void SwapPlaces(Vector2Int currentIndex, Vector2Int targetIndex)
    {
        (_items[currentIndex.x, currentIndex.y], _items[targetIndex.x, targetIndex.y]) =
            (_items[targetIndex.x, targetIndex.y], _items[currentIndex.x, currentIndex.y]);
    }
    
    private void ShowDoubleSwapAnimation(ItemView currentItem, ItemView targetItem, TweenCallback onMovementCompleted)
    { 
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
        var currentItemTweener = currentItem.ChangePosition(targetItem.transform.position);
        var targetItemTweener = targetItem.ChangePosition(currentItem.transform.position);
        
        _tweenersAwaiter.Await(onMovementCompleted, currentItemTweener, targetItemTweener);
    }
}