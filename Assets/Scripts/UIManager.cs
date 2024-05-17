using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [Inject] CardManager cardManager;
    [Inject] GridSelectorElement gridSelector;

    [SerializeField] Button _rebuildBtn;

    void OnEnable()
    {
        _rebuildBtn.onClick.AddListener(OnRebuildBtnClicked);
    }

    void OnRebuildBtnClicked()
    {
        var gridSize = gridSelector.GridSizeInput;
        cardManager.CreateCards(gridSize);
    }
}
