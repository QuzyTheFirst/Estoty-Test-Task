using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ResourceUser : MonoBehaviour
{
    [Serializable]
    public struct BuildRequirement
    {
        public ResourceSO ResourceSO;
        public int Amount;
    }

    public struct ResourceUsedEventData
    {
        public ResourceSO ResourceSO;
        public Transform PlayerTf;
    }

    public event EventHandler<ResourceUsedEventData> ResourceUsed;
    public event EventHandler BuildCompleted; 

    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _intervalBetweenUsing = .2f;
    [SerializeField] private BuildRequirement[] _reqList;

    private Dictionary<ResourceSO, int> _resourcesAmountCollected;

    private Coroutine _playerInTriggerCoroutine;

    public BuildRequirement[] BuildRequirements => _reqList;
    public IReadOnlyDictionary<ResourceSO, int> ResourcesAmountCollected => _resourcesAmountCollected;

    private void Awake()
    {
        _resourcesAmountCollected = new Dictionary<ResourceSO, int>();

        foreach (BuildRequirement buildRequirement in _reqList)
        {
            _resourcesAmountCollected.Add(buildRequirement.ResourceSO, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_playerMask & (1 << other.gameObject.layer)) != 0)
        {
            _playerInTriggerCoroutine = StartCoroutine(GetResources(other.transform));
        }
    }

    IEnumerator GetResources(Transform player)
    {
        while (true)
        {
            bool readyToBeBuilt = true;
            bool isResourceUsed = false;
            foreach (BuildRequirement buildRequirement in _reqList)
            {
                if (_resourcesAmountCollected[buildRequirement.ResourceSO] == buildRequirement.Amount)
                    continue;
                
                readyToBeBuilt = false;
                
                if (!GM.TrySpendResource(buildRequirement.ResourceSO))
                    continue;

                _resourcesAmountCollected[buildRequirement.ResourceSO]++;
                ResourceUsed?.Invoke(this, new ResourceUsedEventData()
                {
                    ResourceSO = buildRequirement.ResourceSO, 
                    PlayerTf = player
                });
                isResourceUsed = true;

                yield return new WaitForSeconds(_intervalBetweenUsing);
            }

            if (!readyToBeBuilt && !isResourceUsed)
                yield break;

            if (!readyToBeBuilt)
                continue;
            
            BuildCompleted?.Invoke(this, EventArgs.Empty);
            enabled = false;
            yield break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_playerMask & (1 << other.gameObject.layer)) != 0)
        {
            if (_playerInTriggerCoroutine != null)
                StopCoroutine(_playerInTriggerCoroutine);
        }
    }
}