using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class CardItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _cardBackgroundSpriteRenderer;
    [SerializeField] SpriteRenderer _cardMarkSignSpriteRenderer;
    [SerializeField] CircleCollider2D _collider;

    Vector2Int _coordinate;
    bool _marked;
    const float _colliderRadiusOffsetFactor = .45f;
    const float _markSignSizeOffsetFactor = .8f;

    void OnMouseDown()
    {
        if (_marked) return;

        Debug.Log("clicked " + _coordinate.x + " : " + _coordinate.y);
        Mark();
    }

    public void Mark()
    {
        _marked = true;
        _cardMarkSignSpriteRenderer.enabled = true;
    }

    public void Init(Vector2Int coordinate, float cardSize, int gridSize, Color cardBackgroundColor, Color cardMarkSignColor, float totalWidthUnit, float totalHeightUnit)
    {
        _coordinate = coordinate;
        _marked = false;

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
