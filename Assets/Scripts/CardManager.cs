using System;
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
    [SerializeField] int _matchThreshold;

    int _currentGridSize;
    int _score;
    List<CardItem> _cards = new List<CardItem>();
    List<CardItem> _connectedMarkedCards = new List<CardItem>();
    HashSet<CardItem> _visitedCards = new HashSet<CardItem>();

    Vector2Int[] directions = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public event Action<int> OnCardsMatched;

    public void RebuildBoard(int gridSize)
    {
        _score = 0;
        CreateCards(gridSize);
    }

    void CreateCards(int gridSize)
    {
        _currentGridSize = gridSize;
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
                cardItem.OnCardClicked += OnCardClickedEvent;
            }
        }
    }

    void ResetConnectedMarkedCards()
    {
        foreach (var connectedMarkedCard in _connectedMarkedCards)
        {
            connectedMarkedCard.UnMark();
            _visitedCards.Remove(connectedMarkedCard);
        }

        _connectedMarkedCards.Clear();
    }

    void OnCardClickedEvent(Vector2Int coordinate)
    {
        _connectedMarkedCards.Clear();
        _visitedCards.Clear();

        FindConnectedMarkedCardsRecursive(coordinate);

        if (_connectedMarkedCards.Count >= _matchThreshold)
        {
            _score++;
            OnCardsMatched?.Invoke(_score);
            ResetConnectedMarkedCards();
        }
    }

    CardItem GetCardAtCoordinate(int x, int y)
    {
        foreach (var card in _cards)
        {
            if (card.Coordinate.x == x && card.Coordinate.y == y)
            {
                return card;
            }
        }

        return null;
    }

    void FindConnectedMarkedCardsRecursive(Vector2Int coordinate)
    {
        var card = GetCardAtCoordinate(coordinate.x, coordinate.y);

        if (card != null && card.IsMarked && !_visitedCards.Contains(card))
        {
            _visitedCards.Add(card);
            _connectedMarkedCards.Add(card);

            foreach (Vector2Int direction in directions)
            {
                var neighborX = coordinate.x + direction.x;
                var neighborY = coordinate.y + direction.y;

                if (neighborX >= 0 && neighborX < _currentGridSize && neighborY >= 0 && neighborY < _currentGridSize)
                {
                    FindConnectedMarkedCardsRecursive(new Vector2Int(neighborX, neighborY));
                }
            }
        }
    }
}
