using DG.Tweening;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    public int Id { get; private set; }
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private Tweener _tweener;

    public void Initialize(ItemModel model)
    {
        Id = model.Id;
        _spriteRenderer.sprite = model.Icon;
    }
}