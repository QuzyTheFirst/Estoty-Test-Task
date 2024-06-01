using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUser : MonoBehaviour
{
    [Serializable]
    public struct BuildRequirement
    {
        public ResourceSO ResourceSO;
        public int Amount;
    }

    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _intervalBetweenUsing = .2f;
    [SerializeField] private BuildRequirement[] _reqList;
    [SerializeField] private GameObject _openingContent;

    private Dictionary<ResourceSO, int> _resourcesAmountCollected;

    private Coroutine _playerInTriggerCoroutine;

    private ResourceUserVisualizer _resourceUserVisualizer;
    

    private void Awake()
    {
        _resourcesAmountCollected = new Dictionary<ResourceSO, int>();
        _resourceUserVisualizer = GetComponent<ResourceUserVisualizer>();

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
                _resourceUserVisualizer.UseResource(buildRequirement.ResourceSO, player, _openingContent.transform.position);
                isResourceUsed = true;

                yield return new WaitForSeconds(_intervalBetweenUsing);
            }

            if (!readyToBeBuilt && !isResourceUsed)
                yield break;

            if (!readyToBeBuilt)
                continue;

            _openingContent.SetActive(true);
            _resourceUserVisualizer.BuiltIsDone();
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