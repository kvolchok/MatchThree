using UnityEngine;

public class MatchThreeController : MonoBehaviour
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
               randomModel.Type == _items[row - 2, column].Type && randomModel.Type == _items[row - 1, column].Type;
    }
    
    private bool IsVerticalMatchThree(Vector2Int index)
    {
        var item = _items[index.x, index.y];

        if (index.x > 1 &&
            item.Type == _items[index.x - 2, index.y].Type && item.Type == _items[index.x - 1, index.y].Type ||
            index.x < _items.GetLength(0) - 2 &&
            item.Type == _items[index.x + 1, index.y].Type && item.Type == _items[index.x + 2, index.y].Type ||
            index.x > 0 && index.x < _items.GetLength(0) - 1 &&
            item.Type == _items[index.x - 1, index.y].Type && item.Type == _items[index.x + 1, index.y].Type)
        {
            return true;
        }

        return false;
    }
    
    private bool IsHorizontalMatchThree(ItemModel randomModel, int row, int column)
    {
        return column > 1 &&
               randomModel.Type == _items[row, column - 2].Type && randomModel.Type == _items[row, column - 1].Type;
    }
    
    private bool IsHorizontalMatchThree(Vector2Int index)
    {
        var item = _items[index.x, index.y];

        if ( index.y > 1 &&
             item.Type == _items[index.x, index.y - 2].Type && item.Type == _items[index.x, index.y - 1].Type ||
             index.y < _items.GetLength(1) - 2 &&
             item.Type == _items[index.x, index.y + 1].Type && item.Type == _items[index.x, index.y + 2].Type ||
             index.y > 0 && index.y < _items.GetLength(1) - 1 &&
             item.Type == _items[index.x, index.y - 1].Type && item.Type == _items[index.x, index.y + 1].Type)
        {
            return true;
        }

        return false;
    }
}