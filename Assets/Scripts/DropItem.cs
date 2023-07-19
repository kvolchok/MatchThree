using UnityEngine;

public struct DropItem
{
    public Vector2Int CurrentCoordinates { get; private set; }
    public Vector2Int TargetCoordinates { get; private set; }

    public DropItem(Vector2Int currentCoordinates, Vector2Int targetCoordinates)
    {
        CurrentCoordinates = currentCoordinates;
        TargetCoordinates = targetCoordinates;
    }
}