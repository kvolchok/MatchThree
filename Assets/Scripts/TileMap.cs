using UnityEngine;

public class TileMap : MonoBehaviour
{
    [field:SerializeField]
    public Vector2Int Size { get; private set; }

    [field:SerializeField]
    public float NextSpawnDistance { get; private set; }
    
    [SerializeField]
    private Transform _tilePrefab;

    private Transform[,] _tiles;

    public void Initialize()
    {
        var halfMapSize = Size / 2;
        var startSpawnPoint = new Vector3(-halfMapSize.x * NextSpawnDistance, halfMapSize.y * NextSpawnDistance,
            transform.position.z);
        
        _tiles = new Transform[Size.x, Size.y];

        for (var x = 0; x < Size.x; x++)
        {
            for (var y = 0; y < Size.y; y++)
            {
                var spawnPosition = startSpawnPoint + new Vector3(y, -x) * NextSpawnDistance;
                var tile = Instantiate(_tilePrefab, transform);
                tile.localPosition = spawnPosition;
                _tiles[x, y] = tile;
            }   
        }
    }

    public Transform[,] GetMap()
    {
        return _tiles;
    }
}