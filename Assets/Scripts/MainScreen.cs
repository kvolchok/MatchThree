using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField]
    private TileMap _tileMap;
    [SerializeField]
    private ItemController _itemController;
    
    [SerializeField]
    private MapIndexProvider _mapIndexProvider;
    [SerializeField]
    private MovementController _movementController;
    [SerializeField]
    private MatchThreeController _matchThreeController;

    private void Awake()
    {
        _tileMap.Initialize();
        _mapIndexProvider.Initialize(_tileMap);
        _itemController.Initialize(_tileMap, _matchThreeController);
        
        var itemViews = _itemController.GetItems();
        _movementController.Initialize(_matchThreeController, _mapIndexProvider, itemViews);
    }
}