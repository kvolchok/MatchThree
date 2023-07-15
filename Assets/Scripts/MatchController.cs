using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchController
{
    private readonly List<Match> _matches = new();
    private readonly List<Vector2Int> _tempMatches = new();

    private ItemView[,] _items;

    public void Initialize(ItemView[,] items)
    {
        _items = items;
    }
    
    public bool IsMatchThreeByModel(ItemModel randomModel, int row, int column)
    {
        return IsVerticalMatch(randomModel, row, column) || IsHorizontalMatch(randomModel, row, column);
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

    public List<Match> GetMatches(Vector2Int currentIndex, Vector2Int targetIndex,
        Action<Vector2Int, Vector2Int> swapPlaces)
    {
        _matches.Clear();
        swapPlaces?.Invoke(currentIndex, targetIndex);

        var currentItem = _items[currentIndex.x, currentIndex.y];
        var targetItem = _items[targetIndex.x, targetIndex.y];

        FindHorizontalMatch(currentIndex, currentItem.Id);
        FindVerticalMatch(currentIndex, currentItem.Id);
        FindHorizontalMatch(targetIndex, targetItem.Id);
        FindVerticalMatch(targetIndex, targetItem.Id);

        swapPlaces?.Invoke(currentIndex, targetIndex);

        return _matches;
    }

    private void FindHorizontalMatch(Vector2Int startIndex, int itemId)
    {
        for (var i = startIndex.y; i < _items.GetLength(0); i++)
        {
            if (_items[startIndex.x, i].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(startIndex.x, i);
            _tempMatches.Add(matchIndex);
        }
        
        for (var i = startIndex.y - 1; i >= 0; i--)
        {
            if (_items[startIndex.x, i].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(startIndex.x, i);
            _tempMatches.Add(matchIndex);
        }

        CheckForMatches();
    }

    private void FindVerticalMatch(Vector2Int startIndex, int itemId)
    {
        for (var i = startIndex.x; i < _items.GetLength(1); i++)
        {
            if (_items[i, startIndex.y].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(i, startIndex.y);
            _tempMatches.Add(matchIndex);
        }
        
        for (var i = startIndex.x - 1; i >= 0; i--)
        {
            if (_items[i, startIndex.y].Id != itemId)
            {
                break;
            }

            var matchIndex = new Vector2Int(i, startIndex.y);
            _tempMatches.Add(matchIndex);
        }
        
        CheckForMatches();
    }
    
    private void CheckForMatches()
    {
        if (_tempMatches.Count < 3)
        {
            _tempMatches.Clear();
        }
        else
        {
            var match = new Match(_tempMatches);
            _matches.Add(match);
        }
        
        _tempMatches.Clear();
    }
}