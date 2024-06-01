using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryUIItem[] _inventoryItems;

    private void Awake()
    {
        foreach (InventoryUIItem inventoryUIItem in _inventoryItems)
        {
            inventoryUIItem.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GM.ResourceAmountChanged += GMOnResourceAmountChanged;
    }

    private void GMOnResourceAmountChanged(object sender, GM.ResourceData resourceData)
    {
        if (_inventoryItems.Any(item => item.CurrentResourceSO == resourceData.ResourceSO))
        {
            InventoryUIItem uiItem = _inventoryItems.First(item => item.CurrentResourceSO == resourceData.ResourceSO);
            uiItem.transform.SetSiblingIndex(0);
            uiItem.Set(resourceData);
        }
        else if (_inventoryItems.Any(item => item.CurrentResourceSO == null))
        {
            InventoryUIItem uiItem = _inventoryItems.First(item => item.CurrentResourceSO == null);
            uiItem.gameObject.SetActive(true);
            uiItem.transform.SetSiblingIndex(0);
            uiItem.Set(resourceData);
        }
        else
        {
            InventoryUIItem uiItem = _inventoryItems.Last();
            uiItem.transform.SetSiblingIndex(0);
            uiItem.Set(resourceData);
        }
    }

    private void OnDisable()
    {
        GM.ResourceAmountChanged -= GMOnResourceAmountChanged;
    }
}
