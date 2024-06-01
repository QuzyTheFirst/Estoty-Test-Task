using System;
using System.Collections.Generic;
using UnityEngine;

public static class GM
{
    public class ResourceData
    {
        public ResourceSO ResourceSO;
        public int Amount;
    }
    private static Dictionary<ResourceSO, int> _resourceAmountDictionary;

    public static event EventHandler<ResourceData> ResourceAmountChanged;
    
    static GM()
    {
        _resourceAmountDictionary = new Dictionary<ResourceSO, int>();
    }

    public static void AddResource(ResourceSO resourceSo)
    {
        _resourceAmountDictionary.TryAdd(resourceSo, 0);
        
        _resourceAmountDictionary[resourceSo]++;
        ResourceAmountChanged?.Invoke(null, new ResourceData(){ResourceSO = resourceSo, Amount = _resourceAmountDictionary[resourceSo]});
    }

    public static bool TrySpendResource(ResourceSO resourceSo)
    {
        if (!_resourceAmountDictionary.ContainsKey(resourceSo))
            return false;

        if (_resourceAmountDictionary[resourceSo] == 0)
            return false;

        _resourceAmountDictionary[resourceSo]--;
        ResourceAmountChanged?.Invoke(null, new ResourceData(){ResourceSO = resourceSo, Amount = _resourceAmountDictionary[resourceSo]});
        return true;
    }
}
