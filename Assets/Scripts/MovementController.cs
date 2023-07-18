using UnityEngine;

public class MovementController : MonoBehaviour
{
    private MatchController _matchController;
    private MapIndexProvider _mapIndexProvider;
    private AnimationsManager _animationsManager;
    private ItemView[,] _items;
    private Camera _camera;
    
    private Vector3 _currentItemPosition;
    private bool _isAnimationPlaying;

    public void Initialize(MatchController matchController, MapIndexProvider mapIndexProvider,
        AnimationsManager animationsManager, ItemView[,] items)
    {
        _matchController = matchController;
        _mapIndexProvider = mapIndexProvider;
        _animationsManager = animationsManager;
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

        TryFindMatchesAfterSwap(currentItemIndex, targetItemIndex);
    }

    private Vector3 GetMovementDirection()
    {
        var targetItemPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var movementDirection = (targetItemPosition - _currentItemPosition).normalized;
        movementDirection.x = Mathf.RoundToInt(movementDirection.x);
        movementDirection.y = Mathf.RoundToInt(movementDirection.y);

        return movementDirection;
    }

    private bool IsAllowedDirection(Vector3 direction, Vector2Int currentIndex)
    {
        return !IsDiagonalDirection(direction) && !IsOutOfRangeDirection(direction, currentIndex);
    }

    private bool IsDiagonalDirection(Vector3 direction)
    {
        return Mathf.Abs(direction.x) - Mathf.Abs(direction.y) == 0;
    }
    
    private bool IsOutOfRangeDirection(Vector3 direction, Vector2Int currentIndex)
    {
        var isOutOfTopBorder = currentIndex.x - direction.y < 0;
        var isOutOfBottomBorder = currentIndex.x - direction.y >= _items.GetLength(0);
        var isOutOfLeftBorder = currentIndex.y + direction.x < 0;
        var isOutOfRightBorder = currentIndex.y + direction.x >= _items.GetLength(1);

        return isOutOfTopBorder || isOutOfBottomBorder || isOutOfLeftBorder || isOutOfRightBorder;
    }
    
    private void TryFindMatchesAfterSwap(Vector2Int currentIndex, Vector2Int targetIndex)
    {
        var currentItem = _items[currentIndex.x, currentIndex.y];
        var targetItem = _items[targetIndex.x, targetIndex.y];

        var matches = _matchController.GetMatchesAfterSwap(currentIndex, targetIndex, SwapPlaces);

        if (matches.Count == 0)
        {
            _animationsManager.ShowDoubleSwapAnimation(currentItem, targetItem, OnAnimationCompleted);
        }
        else
        {
            SwapPlaces(currentIndex, targetIndex);

            _animationsManager.ShowSwapAnimation(currentItem, targetItem, OnAnimationCompleted);
        }
    }

    private void SwapPlaces(Vector2Int currentIndex, Vector2Int targetIndex)
    {
        (_items[currentIndex.x, currentIndex.y], _items[targetIndex.x, targetIndex.y]) =
            (_items[targetIndex.x, targetIndex.y], _items[currentIndex.x, currentIndex.y]);
    }

    private void OnAnimationCompleted()
    {
        _isAnimationPlaying = false;
    }
}