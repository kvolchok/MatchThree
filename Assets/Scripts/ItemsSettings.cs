using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemsSettings
{
    [SerializeField]
    private List<ItemModel> _itemModels;
    
    private List<ItemModel> _tempList = new();

    public void AssignIdToModels()
    {
        for (var i = 0; i < _itemModels.Count; i++)
        {
            var itemModel = _itemModels[i];
            itemModel.SetId(i);
        }
    }

    public List<ItemModel> GetModelsExcept(List<ItemModel> exceptions)
    {
        if (exceptions == null)
        {
            return _itemModels;
        }
        
        _tempList.Clear();
        _tempList.AddRange(_itemModels.Where(itemModel => !exceptions.Contains(itemModel)));

        return _tempList;
    }
}