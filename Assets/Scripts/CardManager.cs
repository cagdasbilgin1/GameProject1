using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CardManager : MonoBehaviour
{
    [Inject] readonly Camera mainCamera;
    [Inject] CardItemFactory cardItemFactory;

    [SerializeField] Transform _board;
    [SerializeField] Color _cardsBackgroundColor;
    [SerializeField] Color _cardsMarkSignColor;

    List<CardItem> _cards;

    private void Start()
    {
        _cards = new List<CardItem>();
    }

    public void CreateCards(int gridSize)
    {
        var totalCardCount = gridSize * gridSize;

        //Object Pooling Start
        if (_cards.Count < totalCardCount)
        {
            var cardsToAdd = totalCardCount - _cards.Count;

            for (int i = 0; i < cardsToAdd; i++)
            {
                var cardItem = cardItemFactory.Create();
                cardItem.transform.SetParent(_board);
                _cards.Add(cardItem);
            }
        }
        else if (_cards.Count > totalCardCount)
        {
            var cardsToRemove = _cards.Count - totalCardCount;

            for (int i = 0; i < cardsToRemove; i++)
            {
                var lastIndex = _cards.Count - 1;
                Destroy(_cards[lastIndex].gameObject);
                _cards.RemoveAt(lastIndex);
            }
        }
        //Object Pooling End

        var totalHeightUnit = mainCamera.orthographicSize * 2;
        var totalWidthUnit = mainCamera.aspect * totalHeightUnit;
        var cardSize = totalWidthUnit / gridSize;

        for (var x = 0; x < gridSize; x++)
        {
            for (var y = 0; y < gridSize; y++)
            {
                var index = x * gridSize + y;
                var cardItem = _cards[index];
                cardItem.Init(new Vector2Int(x, y), cardSize, gridSize, _cardsBackgroundColor, _cardsMarkSignColor, totalWidthUnit, totalHeightUnit);
            }
        }
    }
}
