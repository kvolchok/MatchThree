using UnityEngine;

[CreateAssetMenu(fileName = "ItemModel", menuName = "ScriptableObject/ItemModel", order = 50)]
public class ItemModel : ScriptableObject
{
    [field:SerializeField]
    public ItemType Type { get; private set; }
    [field:SerializeField]
    public Sprite Icon { get; private set; }
}