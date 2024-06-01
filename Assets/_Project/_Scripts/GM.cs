using System.Collections.Generic;
using UnityEngine;

public static class GM
{
    private static Dictionary<ResourceSO, int> _resourceAmountDictionary;

    static GM()
    {
        _resourceAmountDictionary = new Dictionary<ResourceSO, int>();
    }

    public static void AddResource(ResourceSO resourceSo)
    {
        _resourceAmountDictionary.TryAdd(resourceSo, 0);
        
        _resourceAmountDictionary[resourceSo]++;
        Debug.Log($"Name: {resourceSo.name} | Amount: {_resourceAmountDictionary[resourceSo]}");
    }

    public static bool TrySpendResource(ResourceSO resourceSo)
    {
        if (!_resourceAmountDictionary.ContainsKey(resourceSo))
            return false;

        if (_resourceAmountDictionary[resourceSo] == 0)
            return false;

        _resourceAmountDictionary[resourceSo]--;
        return true;
    }
}
