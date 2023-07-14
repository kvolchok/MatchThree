using UnityEngine;

[CreateAssetMenu(fileName = "ItemModel", menuName = "ScriptableObject/ItemModel", order = 50)]
public class ItemModel : ScriptableObject
{
    public int Id { get; private set; }
    
    [field:SerializeField]
    public Sprite Icon { get; private set; }

    public void SetId(int id)
    {
        Id = id;
    }
}