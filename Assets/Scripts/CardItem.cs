using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class CardItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _cardBackgroundSpriteRenderer;
    [SerializeField] SpriteRenderer _cardMarkSignSpriteRenderer;
    [SerializeField] CircleCollider2D _collider;

    Vector2Int _coordinate;
    bool _isMarked;
    const float _colliderRadiusOffsetFactor = .45f;
    const float _markSignSizeOffsetFactor = .8f;

    public event Action<Vector2Int> OnCardClicked;

    public Vector2Int Coordinate => _coordinate;
    public bool IsMarked => _isMarked;

    void OnMouseDown()
    {
        if (_isMarked) return;

        Mark();
        OnCardClicked?.Invoke(_coordinate);
    }

    public void Mark()
    {
        _isMarked = true;
        _cardMarkSignSpriteRenderer.enabled = true;
    }

    public void UnMark()
    {
        _isMarked = false;
        _cardMarkSignSpriteRenderer.enabled = false;
    }

    public void Init(Vector2Int coordinate, float cardSize, int gridSize, Color cardBackgroundColor, Color cardMarkSignColor, float totalWidthUnit, float totalHeightUnit)
    {
        _coordinate = coordinate;
        _isMarked = false;

        var halfGridSize = (float) gridSize / 2;
        var offsetX = (_coordinate.x - halfGridSize + 0.5f) * cardSize;
        var offsetY = (_coordinate.y - halfGridSize + 0.5f) * cardSize + (totalHeightUnit - totalWidthUnit) / 2;
        
        transform.position = new Vector2(offsetX, offsetY);
        _cardBackgroundSpriteRenderer.size = new Vector2(cardSize, cardSize);
        _cardMarkSignSpriteRenderer.size = new Vector2(cardSize, cardSize) * _markSignSizeOffsetFactor;
        _collider.radius = cardSize * _colliderRadiusOffsetFactor;
        _cardBackgroundSpriteRenderer.color = cardBackgroundColor;
        _cardMarkSignSpriteRenderer.color = cardMarkSignColor;
        _cardMarkSignSpriteRenderer.enabled = false;
    }
}
