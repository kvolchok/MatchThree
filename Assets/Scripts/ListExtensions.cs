using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandomElement<T>(this IList<T> list)
    {
        var randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
}