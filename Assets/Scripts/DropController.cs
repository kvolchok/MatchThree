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
                    var holesInColumnCount = _holesInColumns[y];
                    holesInColumnCount++;
                    _holesInColumns[y] = holesInColumnCount;
                }
            }   
        }
    }
    
    public List<DropItem> GetDropItems()
    {
        foreach (var (columnNumber, holesCount) in _holesInColumns)
        {
            for (var x = 0; x < _items.GetLength(0); x++)
            {
                if (_items[x, columnNumber].IsMatched)
                {
                    break;
                }
                
                var currentCoordinates = new Vector2Int(x, columnNumber);
                var targetCoordinates = new Vector2Int(x + holesCount, columnNumber);
                var dropItem = new DropItem(currentCoordinates, targetCoordinates);
                _dropItems.Add(dropItem);
            }   
        }
        
        return _dropItems;
    }
}