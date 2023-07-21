using System.Collections.Generic;
using UnityEngine;

public class DropController
{
    private readonly Dictionary<int, int> _holesInColumns = new();
    private readonly List<DropItem> _dropItems = new();

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
                // if (_items[x, y] == null)
                // {
                //     continue;
                // }
                
                // if (!_items[x, y].IsMatched || !_items[x, y].enabled)
                // {
                //     continue;
                // }
                
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
                    var holesInColumnCount = _holesInColumns[y];
                    holesInColumnCount++;
                    _holesInColumns[y] = holesInColumnCount;
                }
            }   
        }
    }
    
    public List<DropItem> GetDropItems()
    {
        _dropItems.Clear();

        foreach (var (columnNumber, holesCount) in _holesInColumns)
        {
            var matchesInColumnCounter = holesCount;
            
            for (var x = 0; x < _items.GetLength(0); x++)
            {
                if (matchesInColumnCounter < 0)
                {
                    break;
                }
                
                // if (_items[x, columnNumber] == null)
                // {
                //     continue;
                // }
                
                if (_items[x, columnNumber].IsMatched)
                {
                    matchesInColumnCounter--;
                    continue;
                }

                var currentCoordinates = new Vector2Int(x, columnNumber);
                var targetCoordinates = new Vector2Int(x + matchesInColumnCounter, columnNumber);
                var dropItem = new DropItem(currentCoordinates, targetCoordinates);
                _dropItems.Add(dropItem);
            }   
        }
        
        return _dropItems;
    }
}