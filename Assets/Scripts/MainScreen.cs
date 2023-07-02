using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField]
    private TileMap _tileMap;
    [SerializeField]
    private ItemController _itemController;

    private void Awake()
    {
        _tileMap.Initialize();
        _itemController.Initialize(_tileMap);
    }
}