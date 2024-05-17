using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [Inject] CardManager cardManager;
    [Inject] GridSelectorElement gridSelector;

    [SerializeField] Button _rebuildBtn;
    [SerializeField] TextMeshProUGUI matchedCountUI;

    void Start()
    {
        ResetMatchedCountUI();
    }

    void OnEnable()
    {
        cardManager.OnCardsMatched += OnCardsMatchedEvent;
        _rebuildBtn.onClick.AddListener(OnRebuildBtnClicked);
    }

    void OnRebuildBtnClicked()
    {
        ResetMatchedCountUI();
        var gridSize = gridSelector.GridSizeInput;
        cardManager.RebuildBoard(gridSize);
    }

    void OnCardsMatchedEvent(int score)
    {
        UpdateMatchedCountUI(score);
    }

    void UpdateMatchedCountUI(int score)
    {
        matchedCountUI.text = score.ToString();
    }

    void ResetMatchedCountUI()
    {
        matchedCountUI.text = "0";
    }
}
