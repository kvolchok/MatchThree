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
        _itemsController.Initialize(_matchController, _animationsManager, _tileMap);
        _itemsController.SpawnNewItems();
        
        var itemViews = _itemsController.GetItems();
        _movementController.Initialize(_matchController, _mapIndexProvider, _animationsManager, itemViews);
        
        _movementController.OnMatchesNotFound += OnMatchesNotFound;
    }

    private void OnMatchesNotFound()
    {
        _itemsController.RemoveMatchedItems();
        _itemsController.SpawnNewItems();
    }
    
    private void OnDestroy()
    {
        _movementController.OnMatchesNotFound -= OnMatchesNotFound;
    }
}