using UnityEngine;

public class MatchThreeController
{
    private ItemView[,] _items;

    public void Initialize(ItemView[,] items)
    {
        _items = items;
    }
    
    public bool IsMatchThreeByModel(ItemModel randomModel, int row, int column)
    {
        return IsVerticalMatchThree(randomModel, row, column) || IsHorizontalMatchThree(randomModel, row, column);
    }
    
    public bool IsMatchThreeByIndex(Vector2Int index)
    {
        return IsVerticalMatchThree(index) || IsHorizontalMatchThree(index);
    }

    private bool IsVerticalMatchThree(ItemModel randomModel, int row, int column)
    {
        return row > 1 &&
               randomModel.Id == _items[row - 2, column].Id && randomModel.Id == _items[row - 1, column].Id;
    }
    
    private bool IsVerticalMatchThree(Vector2Int index)
    {
        var item = _items[index.x, index.y];

        if (index.x > 1 &&
            item.Id == _items[index.x - 2, index.y].Id && item.Id == _items[index.x - 1, index.y].Id ||
            index.x < _items.GetLength(0) - 2 &&
            item.Id == _items[index.x + 1, index.y].Id && item.Id == _items[index.x + 2, index.y].Id ||
            index.x > 0 && index.x < _items.GetLength(0) - 1 &&
            item.Id == _items[index.x - 1, index.y].Id && item.Id == _items[index.x + 1, index.y].Id)
        {
            return true;
        }

        return false;
    }
    
    private bool IsHorizontalMatchThree(ItemModel randomModel, int row, int column)
    {
        return column > 1 &&
               randomModel.Id == _items[row, column - 2].Id && randomModel.Id == _items[row, column - 1].Id;
    }
    
    private bool IsHorizontalMatchThree(Vector2Int index)
    {
        var item = _items[index.x, index.y];

        if ( index.y > 1 &&
             item.Id == _items[index.x, index.y - 2].Id && item.Id == _items[index.x, index.y - 1].Id ||
             index.y < _items.GetLength(1) - 2 &&
             item.Id == _items[index.x, index.y + 1].Id && item.Id == _items[index.x, index.y + 2].Id ||
             index.y > 0 && index.y < _items.GetLength(1) - 1 &&
             item.Id == _items[index.x, index.y - 1].Id && item.Id == _items[index.x, index.y + 1].Id)
        {
            return true;
        }

        return false;
    }
}