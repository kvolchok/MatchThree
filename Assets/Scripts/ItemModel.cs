using UnityEngine;

[CreateAssetMenu(fileName = "ItemModel", menuName = "ScriptableObject/ItemModel", order = 50)]
public class ItemModel : ScriptableObject
{
    [field:SerializeField]
    public Sprite Icon { get; private set; }
    
    public int Id { get; private set; }

    public void SetId(int id)
    {
        Id = id;
    }
}