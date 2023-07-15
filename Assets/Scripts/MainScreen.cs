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
    
    private MatchController _matchController;

    private void Awake()
    {
        _tileMap.Initialize();
        _mapIndexProvider.Initialize(_tileMap);

        _matchController = new MatchController();
        _itemController.Initialize(_tileMap, _matchController);
        
        var itemViews = _itemController.GetItems();
        _movementController.Initialize(_matchController, _mapIndexProvider, itemViews);
    }
}