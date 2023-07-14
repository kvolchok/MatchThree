using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private readonly List<ItemModel> _exceptions = new();
    
    [SerializeField]
    private ItemSettings _itemSettings;
    [SerializeField]
    private ItemView _itemPrefab;
    
    [SerializeField]
    private MatchThreeController _matchThreeController;
    [SerializeField]
    private MapIndexProvider _mapIndexProvider;
    [SerializeField]
    private MovementController _movementController;

    private ItemView[,] _items;

    public void Initialize(TileMap tileMap)
    {
        _itemSettings.AssignIdToModels();
        
        var map = tileMap.GetMap();
        _items = new ItemView[tileMap.Size.x, tileMap.Size.y];
        _matchThreeController.Initialize(_items);
        
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
        
        _mapIndexProvider.Initialize(tileMap);
        _movementController.Initialize(_matchThreeController, _mapIndexProvider, _items);
    }

    private void ShowItem(ItemView item, int row, int column, List<ItemModel> exceptions)
    {
        var modelsExcept = _itemSettings.GetModelsExcept(exceptions);
        var randomModel = modelsExcept.GetRandomElement();

        if (_matchThreeController.IsMatchThreeByModel(randomModel, row, column))
        {
            exceptions.Add(randomModel);
            
            ShowItem(item, row, column, exceptions);
        }
        else
        {
            item.Initialize(randomModel);
        }
    }
}