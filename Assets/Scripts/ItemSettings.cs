using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ItemSettings
{
    [SerializeField]
    private ItemModel[] _itemModels;

    public ItemModel GetRandomModel()
    {
        var randomIndex = Random.Range(0, _itemModels.Length);
        return _itemModels[randomIndex];
    }
    
    public ItemModel GetRandomModelExcept(ItemModel model)
    {
        while (true)
        {
            var randomIndex = Random.Range(0, _itemModels.Length);
            if (_itemModels[randomIndex].Type != model.Type)
            {
                return _itemModels[randomIndex];
            }
        }
    }
}