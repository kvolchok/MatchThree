using System.Collections.Generic;

public struct Match
{
    public List<ItemView> Items { get; private set; }

    public void Add(ItemView itemView)
    {
        Items ??= new List<ItemView>();
        
        Items.Add(itemView);
    }
}