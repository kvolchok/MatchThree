using System.Collections.Generic;
using UnityEngine;

public readonly struct Match
{
    private readonly List<Vector2Int> _indices;

    public Match(List<Vector2Int> indices)
    {
        _indices = indices;
    }
}