using System.Collections.Generic;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
    private readonly List<ItemModel> _exceptions = new();

    [SerializeField]
    private ItemsSettings _itemsSettings;
    [SerializeField]
    private ItemView _itemPrefab;

    private MatchController _matchController;
    private AnimationsManager _animationsManager;
    private ItemView[,] _items;
    private Transform[,] _map;

    public void Initialize(MatchController matchController, AnimationsManager animationsManager, TileMap tileMap)
    {
        _matchController = matchController;
        _animationsManager = animationsManager;
        _itemsSettings.AssignIdToModels();

        _map = tileMap.GetMap();
        _items = new ItemView[tileMap.Size.x, tileMap.Size.y];
        _matchController.Initialize(_items);
    }
    
    public void SpawnNewItems()
    {
        for (var x = 0; x < _items.GetLength(0); x++)
        {
            for (var y = 0; y < _items.GetLength(1); y++)
            {
                if (_items[x, y] != null && _items[x, y].enabled)
                {
                    continue;
                }
                
                var item = Instantiate(_itemPrefab, transform);
                item.transform.position = _map[x, y].position;
                _items[x, y] = item;

                _exceptions.Clear();
                ShowItem(item, x, y, _exceptions);
            }
        }
    }

    public ItemView[,] GetItems()
    {
        return _items;
    }

    public void RemoveMatchedItems()
    {
        foreach (var itemView in _items)
        {
            if (itemView.IsMatched)
            {
                Destroy(itemView.gameObject);
            }
        }
    }

    private void ShowItem(ItemView item, int row, int column, List<ItemModel> exceptions)
    {
        var modelsExcept = _itemsSettings.GetModelsExcept(exceptions);
        var randomModel = modelsExcept.GetRandomElement();

        if (_matchController.IsMatchByModel(randomModel, row, column))
        {
            exceptions.Add(randomModel);

            ShowItem(item, row, column, exceptions);
        }
        else
        {
            item.Initialize(randomModel);
            _animationsManager.ShowAppearItemAnimation(item);
        }
    }
}