using System.Collections.Generic;

public readonly struct Match
{
    public List<ItemView> Items { get; }

    public Match(List<ItemView> items)
    {
        Items = items;
    }
}