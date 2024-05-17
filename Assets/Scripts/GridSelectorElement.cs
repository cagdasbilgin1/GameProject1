using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSelectorElement : MonoBehaviour
{
    private const int MIN_GRID_SIZE = 3;
    private const int MAX_GRID_SIZE = 8;

    [SerializeField] Button _decreaseBtn; 
    [SerializeField] Button _increaseBtn;
    [SerializeField] TextMeshProUGUI _gridSizeInputUI;
    [SerializeField][Range(MIN_GRID_SIZE, MAX_GRID_SIZE)] int _gridSizeInput;

    public int GridSizeInput => _gridSizeInput;

    void Start()
    {
        _gridSizeInputUI.text = _gridSizeInput.ToString();
    }

    void OnEnable()
    {
        _decreaseBtn.onClick.AddListener(DecreaseGridSizeInput);
        _increaseBtn.onClick.AddListener(IncreaseGridSizeInput);
    }

    void DecreaseGridSizeInput()
    {
        _gridSizeInput--;
        _gridSizeInput = Mathf.Clamp(_gridSizeInput, MIN_GRID_SIZE, MAX_GRID_SIZE);

        _gridSizeInputUI.text = _gridSizeInput.ToString();
    }

    void IncreaseGridSizeInput()
    {
        _gridSizeInput++;
        _gridSizeInput = Mathf.Clamp(_gridSizeInput, 3, 8);

        _gridSizeInputUI.text = _gridSizeInput.ToString();
    }
}
