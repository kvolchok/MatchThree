using UnityEngine;

public class ItemView : MonoBehaviour
{
    public int Id { get; private set; }
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public void Initialize(ItemModel model)
    {
        Id = model.Id;
        _spriteRenderer.sprite = model.Icon;
    }

    public void DestroyItemView()
    {
        Destroy(gameObject);
    }
}