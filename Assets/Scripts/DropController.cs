using System.Collections.Generic;
using UnityEngine;

public class DropController
{
    private readonly Dictionary<int, int> _holesInColumns = new();
    private readonly List<DropItem> _itemsToDrop = new();

    private readonly ItemView[,] _items;

    public DropController(ItemView[,] items)
    {
        _items = items;
    }
    
    public void CalculateHolesInColumns()
    {
        _holesInColumns.Clear();
        
        for (var y = 0; y < _items.GetLength(1); y++)
        {
            for (var x = 0; x < _items.GetLength(0); x++)
            {
                if (!_items[x, y].IsMatched)
                {
                    continue;
                }
                
                if (!_holesInColumns.ContainsKey(y))
                {
                    _holesInColumns.Add(y, 1);
                }
                else
                {
                    var holesCount = _holesInColumns[y];
                    holesCount++;
                    _holesInColumns[y] = holesCount;
                }
            }   
        }
    }
    
    public List<DropItem> GetItemsToDrop()
    {
        _itemsToDrop.Clear();

        foreach (var (columnNumber, holesCount) in _holesInColumns)
        {
            var matchesInColumnCount = holesCount;
            
            for (var x = 0; x < _items.GetLength(0); x++)
            {
                if (matchesInColumnCount <= 0)
                {
                    break;
                }

                if (_items[x, columnNumber].IsMatched)
                {
                    matchesInColumnCount--;
                    continue;
                }

                var currentCoordinates = new Vector2Int(x, columnNumber);
                var targetCoordinates = new Vector2Int(x + matchesInColumnCount, columnNumber);
                var dropItem = new DropItem(currentCoordinates, targetCoordinates);
                _itemsToDrop.Add(dropItem);
            }   
        }
        
        return _itemsToDrop;
    }
}