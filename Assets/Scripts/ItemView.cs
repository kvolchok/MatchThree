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

    public void Initialize(ItemModel model)
    {
        Id = model.Id;
        _spriteRenderer.sprite = model.Icon;

        transform.DOScale(_endScale, _scaleDuration);
    }

    public void ChangePosition(Vector3 toPosition)
    {
        transform.DOMove(toPosition, _movementDuration);
    }
    
    public void ChangePosition(Vector3[] path)
    {
        transform.DOPath(path, _movementDuration);
    }
}