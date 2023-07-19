using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private readonly List<ItemModel> _exceptions = new();

    [SerializeField]
    private ItemSettings _itemSettings;
    [SerializeField]
    private ItemView _itemPrefab;

    private MatchController _matchController;
    private AnimationsManager _animationsManager;
    private ItemView[,] _items;

    public void Initialize(MatchController matchController, AnimationsManager animationsManager)
    {
        _matchController = matchController;
        _animationsManager = animationsManager;
        _itemSettings.AssignIdToModels();
    }

    public ItemView[,] GetItems(TileMap tileMap)
    {
        var map = tileMap.GetMap();
        _items = new ItemView[tileMap.Size.x, tileMap.Size.y];
        _matchController.Initialize(_items);

        for (var x = 0; x < tileMap.Size.x; x++)
        {
            for (var y = 0; y < tileMap.Size.y; y++)
            {
                var item = Instantiate(_itemPrefab, transform);
                item.transform.position = map[x, y].position;
                _items[x, y] = item;

                _exceptions.Clear();
                ShowItem(item, x, y, _exceptions);
            }
        }
        
        return _items;
    }

    private void ShowItem(ItemView item, int row, int column, List<ItemModel> exceptions)
    {
        var modelsExcept = _itemSettings.GetModelsExcept(exceptions);
        var randomModel = modelsExcept.GetRandomElement();

        if (_matchController.IsMatchThreeByModel(randomModel, row, column))
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