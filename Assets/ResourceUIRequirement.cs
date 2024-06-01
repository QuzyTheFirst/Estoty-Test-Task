using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIRequirement : MonoBehaviour
{
    [SerializeField] private Image _resourceImage;
    [SerializeField] private TextMeshProUGUI _resourcesCollectedText;
    
    private ResourceSO _currentResourceSO;
    private int _amountToCollect;

    public ResourceSO CurrentResourceSO => _currentResourceSO;

    public void Initialize(ResourceSO resourceSo, int amountToCollect)
    {
        _resourceImage.color = resourceSo.UIColor;
        _resourcesCollectedText.text = $"0 / {amountToCollect}";
        
        _currentResourceSO = resourceSo;
        _amountToCollect = amountToCollect;
    }

    public void UpdateCollectedAmount(int collectedAmount)
    {
        _resourcesCollectedText.text = $"{collectedAmount} / {_amountToCollect}";
    }
}
