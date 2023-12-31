using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchController
{
    private readonly List<Match> _matches = new();

    private ItemView[,] _items;

    public void Initialize(ItemView[,] items)
    {
        _items = items;
    }
    
    public bool IsMatchByModel(ItemModel randomModel, int row, int column)
    {
        return IsHorizontalMatch(randomModel, row, column) || IsVerticalMatch(randomModel, row, column);
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
    
    public List<Match> GetAllPossibleMatches()
    {
        _matches.Clear();
        
        for (var x = 0; x < _items.GetLength(0); x++)
        {
            for (var y = 0; y < _items.GetLength(1); y++)
            {
                var currentItem = _items[x, y];
                if (currentItem.IsMatched)
                {
                    continue;
                }

                var currentIndex = new Vector2Int(x, y);
                var match = GetHorizontalMatch(currentIndex, currentItem.Id);
                TryAddMatch(match);

                match = GetVerticalMatch(currentIndex, currentItem.Id);
                TryAddMatch(match);
            }   
        }

        return _matches;
    }

    private bool IsHorizontalMatch(ItemModel randomModel, int row, int column)
    {
        return column > 1 &&
               randomModel.Id == _items[row, column - 2].Id && randomModel.Id == _items[row, column - 1].Id ||
               column < _items.GetLength(1) - 2 &&
               _items[row, column + 1] != null && _items[row, column + 2] != null &&
               randomModel.Id == _items[row, column + 1].Id && randomModel.Id == _items[row, column + 2].Id || 
               column > 0 && column < _items.GetLength(1) - 1 &&
               randomModel.Id == _items[row, column - 1].Id &&
               _items[row, column + 1] != null && randomModel.Id == _items[row, column + 1].Id;
    }
    
    private bool IsVerticalMatch(ItemModel randomModel, int row, int column)
    {
        return row > 1 &&
               randomModel.Id == _items[row - 2, column].Id && randomModel.Id == _items[row - 1, column].Id ||
               row < _items.GetLength(0) - 2 &&
               _items[row + 1, column] != null && _items[row + 2, column] != null &&
               randomModel.Id == _items[row + 1, column].Id && randomModel.Id == _items[row + 2, column].Id || 
               row > 0 && row < _items.GetLength(0) - 1 &&
               randomModel.Id == _items[row - 1, column].Id &&
               _items[row + 1, column] != null && randomModel.Id == _items[row + 1, column].Id;
    }

    private Match GetHorizontalMatch(Vector2Int startIndex, int itemId)
    {
        var match = new Match();
        
        for (var i = startIndex.y; i < _items.GetLength(1); i++)
        {
            if (_items[startIndex.x, i].IsMatched || _items[startIndex.x, i].Id != itemId)
            {
                break;
            }
            
            var itemView = _items[startIndex.x, i];
            match.Add(itemView);
        }
        
        for (var i = startIndex.y - 1; i >= 0; i--)
        {
            if (_items[startIndex.x, i].IsMatched || _items[startIndex.x, i].Id != itemId)
            {
                break;
            }
            
            var itemView = _items[startIndex.x, i];
            match.Add(itemView);
        }

        return match;
    }

    private Match GetVerticalMatch(Vector2Int startIndex, int itemId)
    {
        var match = new Match();

        for (var i = startIndex.x; i < _items.GetLength(0); i++)
        {
            if (_items[i, startIndex.y].IsMatched || _items[i, startIndex.y].Id != itemId)
            {
                break;
            }
            
            var itemView = _items[i, startIndex.y];
            match.Add(itemView);
        }
        
        for (var i = startIndex.x - 1; i >= 0; i--)
        {
            if (_items[i, startIndex.y].IsMatched || _items[i, startIndex.y].Id != itemId)
            {
                break;
            }
            
            var itemView = _items[i, startIndex.y];
            match.Add(itemView);
        }

        return match;
    }
    
    private void TryAddMatch(Match match)
    {
        if (match.Items == null)
        {
            return;
        }
        
        if (!HasEnoughMatchesCount(match))
        {
            return;
        }

        MarkMatchedItems(match);
        _matches.Add(match);
    }

    private bool HasEnoughMatchesCount(Match match)
    {
        return match.Items.Count >= 3;
    }
    
    private void MarkMatchedItems(Match match)
    {
        foreach (var matchItem in match.Items)
        {
            matchItem.ChangeMatchState(true);
        }
    }
}