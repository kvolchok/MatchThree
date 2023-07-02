using UnityEngine;

public class MapIndexProvider : MonoBehaviour
{
    private TileMap _tileMap;

    public void Initialize(TileMap tileMap)
    {
        _tileMap = tileMap;
    }

    public Vector2Int GetIndex(Vector3 worldPosition)
    {
        var pointPosition = transform.InverseTransformPoint(worldPosition);
        pointPosition.z = transform.position.z;

        var x = Mathf.RoundToInt(pointPosition.x / _tileMap.NextSpawnDistance);
        var y = Mathf.RoundToInt(pointPosition.y / -_tileMap.NextSpawnDistance);

        var halfMapSize = new Vector2Int(_tileMap.Size.x, _tileMap.Size.y) / 2;
        var mapIndex = new Vector2Int(y, x) + halfMapSize;
        
        return mapIndex;
    }
    
    public Vector2Int GetTargetItemIndex(Vector2Int index, Vector3 direction)
    {
        var x = -(int)direction.y;
        var y = (int)direction.x;

        return index + new Vector2Int(x, y);
    }
}