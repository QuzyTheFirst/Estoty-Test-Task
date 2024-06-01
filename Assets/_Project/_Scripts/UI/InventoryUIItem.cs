using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    [SerializeField] private Image _resourceImage;
    [SerializeField] private TextMeshProUGUI _resourceAmount;

    private ResourceSO _currentResourceSO;

    public ResourceSO CurrentResourceSO => _currentResourceSO;

    public void Set(GM.ResourceData resourceData)
    {
        _resourceImage.color = resourceData.ResourceSO.UIColor;
        _resourceAmount.text = $"{resourceData.Amount}";
        _currentResourceSO = resourceData.ResourceSO;
    }
}