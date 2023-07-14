using DG.Tweening;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    public int Id { get; private set; }
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _endScale;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private float _movementDuration;

    private Tweener _tweener;

    public void Initialize(ItemModel model)
    {
        Id = model.Id;
        _spriteRenderer.sprite = model.Icon;

        transform.DOScale(_endScale, _scaleDuration);
    }

    public Tweener ChangePosition(Vector3 toPosition)
    {
        _tweener = transform.DOMove(toPosition, _movementDuration);
        
        return _tweener;
    }
    
    public Tweener ChangePosition(Vector3[] path)
    {
        _tweener = transform.DOPath(path, _movementDuration);
        
        return _tweener;
    }
}