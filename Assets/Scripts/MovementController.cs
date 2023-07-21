using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovementController : MonoBehaviour
{ 
    public event Action OnMatchesNotFound;
    
    private MatchController _matchController;
    private MapIndexProvider _mapIndexProvider;
    private AnimationsManager _animationsManager;
    
    private ItemView[,] _items;
    private DropController _dropController;
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
        _dropController = new DropController(_items);
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
        
        _isAnimationPlaying = true;
        if (matches.Count == 0)
        {
            _animationsManager.ShowDoubleSwapAnimation(currentItem, targetItem, OnAnimationCompleted);
        }
        else
        {
            SwapPlaces(currentIndex, targetIndex);
            _animationsManager.ShowMatchAnimation(currentItem, targetItem, matches, OnItemsMatched);
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

    private void OnItemsMatched()
    {
        _dropController.CalculateHolesInColumns();
        var itemsToDrop = _dropController.GetItemsToDrop();
        DropItems(itemsToDrop);
    }

    private void DropItems(List<DropItem> dropItems)
    {
        _animationsManager.ShowDropItemsAnimation(dropItems, _items, TryFindMatchesAfterDropping);

        for (var i = dropItems.Count - 1; i >= 0; i--)
        {
            var dropItem = dropItems[i];
            SwapPlaces(dropItem.TargetCoordinates, dropItem.CurrentCoordinates);
        }
    }

    private void TryFindMatchesAfterDropping()
    {
        var allPossibleMatches = _matchController.GetAllPossibleMatches();
        
        if (allPossibleMatches.Count != 0)
        {
            var matchSequence = _animationsManager.GetMatchSequence(allPossibleMatches);
            matchSequence.OnComplete(OnItemsMatched);
        }
        else
        {
            OnMatchesNotFound?.Invoke();
            OnAnimationCompleted();
        }
    }
}