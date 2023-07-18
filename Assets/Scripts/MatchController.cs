using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MatchController
{
    private readonly List<Match> _matches = new();

    private ItemView[,] _items;
    private Tweener _itemTweener;

    public void Initialize(ItemView[,] items)
    {
        _items = items;
    }
    
    public bool IsMatchThreeByModel(ItemModel randomModel, int row, int column)
    {
        return IsVerticalMatch(randomModel, row, column) || IsHorizontalMatch(randomModel, row, column);
    }
    
    public List<Match> GetMatchesAfterSwap(Vector2Int currentIndex, Vector2Int targetIndex,
        Action<Vector2Int, Vector2Int> swapPlaces)
    {
        _matches.Clear();
        swapPlaces?.Invoke(currentIndex, targetIndex);

        var currentItem = _items[currentIndex.x, currentIndex.y];
        var targetItem = _items[targetIndex.x, targetIndex.y];

        var match = GetHorizontalMatch(currentIndex, currentItem.Id);
        TryAddMatch(match);

        match = GetVerticalMatch(currentIndex, currentItem.Id);
        TryAddMatch(match);

        match = GetHorizontalMatch(targetIndex, targetItem.Id);
        TryAddMatch(match);

        match = GetVerticalMatch(targetIndex, targetItem.Id);
        TryAddMatch(match);

        swapPlaces?.Invoke(currentIndex, targetIndex);

        return _matches;
    }

    private bool IsVerticalMatch(ItemModel randomModel, int row, int column)
    {
        return row > 1 &&
               randomModel.Id == _items[row - 2, column].Id && randomModel.Id == _items[row - 1, column].Id;
    }

    private bool IsHorizontalMatch(ItemModel randomModel, int row, int column)
    {
        return column > 1 &&
               randomModel.Id == _items[row, column - 2].Id && randomModel.Id == _items[row, column - 1].Id;
    }

    private Match GetHorizontalMatch(Vector2Int startIndex, int itemId)
    {
        var match = new Match();
        
        for (var i = startIndex.y; i < _items.GetLength(0); i++)
        {
            if (_items[startIndex.x, i].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(startIndex.x, i);
            var itemView = _items[matchIndex.x, matchIndex.y];
            match.Add(itemView);
        }
        
        for (var i = startIndex.y - 1; i >= 0; i--)
        {
            if (_items[startIndex.x, i].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(startIndex.x, i);
            var itemView = _items[matchIndex.x, matchIndex.y];
            match.Add(itemView);
        }

        return match;
    }

    private Match GetVerticalMatch(Vector2Int startIndex, int itemId)
    {
        var match = new Match();

        for (var i = startIndex.x; i < _items.GetLength(1); i++)
        {
            if (_items[i, startIndex.y].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(i, startIndex.y);
            var itemView = _items[matchIndex.x, matchIndex.y];
            match.Add(itemView);
        }
        
        for (var i = startIndex.x - 1; i >= 0; i--)
        {
            if (_items[i, startIndex.y].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(i, startIndex.y);
            var itemView = _items[matchIndex.x, matchIndex.y];
            match.Add(itemView);
        }

        return match;
    }
    
    private void TryAddMatch(Match match)
    {
        if (HasEnoughMatchesCount(match))
        {
            _matches.Add(match);
        }
    }
    
    private bool HasEnoughMatchesCount(Match match)
    {
        return match.Items.Count >= 3;
    }
}