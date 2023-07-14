using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ItemSettings
{
    [SerializeField]
    private ItemModel[] _itemModels;

    public void AssignIdToModels()
    {
        for (var i = 0; i < _itemModels.Length; i++)
        {
            var itemModel = _itemModels[i];
            itemModel.SetId(i);
        }
    }
    
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
            if (_itemModels[randomIndex].Id != model.Id)
            {
                return _itemModels[randomIndex];
            }
        }
    }
}