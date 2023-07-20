using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TileMap _tileMap;
    [SerializeField]
    private ItemsController _itemsController;
    
    [SerializeField]
    private MapIndexProvider _mapIndexProvider;
    [SerializeField]
    private MovementController _movementController;
    [SerializeField]
    private AnimationsManager _animationsManager;
    
    private MatchController _matchController;

    private void Awake()
    {
        _tileMap.Initialize();
        _mapIndexProvider.Initialize(_tileMap);

        _matchController = new MatchController();
        _itemsController.Initialize(_matchController, _animationsManager);
        
        var itemViews = _itemsController.GetItems(_tileMap);
        _movementController.Initialize(_matchController, _mapIndexProvider, _animationsManager, itemViews);
        
        _movementController.OnDropItems += OnDropItems;
    }

    private void OnDropItems()
    {
        _itemsController.RemoveMatchedItems();
    }

    private void OnDestroy()
    {
        _movementController.OnDropItems -= OnDropItems;
    }
}