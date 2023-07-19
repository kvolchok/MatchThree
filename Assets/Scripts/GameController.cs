using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TileMap _tileMap;
    [SerializeField]
    private ItemSpawner _itemSpawner;
    
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
        _itemSpawner.Initialize(_matchController, _animationsManager);
        
        var itemViews = _itemSpawner.GetItems(_tileMap);
        _movementController.Initialize(_matchController, _mapIndexProvider, _animationsManager, itemViews);
    }
}