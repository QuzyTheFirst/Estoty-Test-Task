using System;
using System.Collections;
using UnityEngine;

public class ResourceGiver : MonoBehaviour
{
    public struct ResourceGivenEventData
    {
        public ResourceSO ResourceSO;
        public Transform PlayerTf;
    }
    public event EventHandler<ResourceGivenEventData> ResourceGiven;
    
    [SerializeField] private ResourceSO _resourceSO;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _intervalBetweenDrop = .2f;

    private Coroutine _playerInTriggerCoroutine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (( _playerMask & (1 << other.gameObject.layer)) != 0)
        {
            _playerInTriggerCoroutine = StartCoroutine(DropResources(other.transform));
        }
    }

    IEnumerator DropResources(Transform player)
    {
        while (true)
        {
            GM.AddResource(_resourceSO);
            ResourceGiven?.Invoke(this, new ResourceGivenEventData()
            {
                ResourceSO = _resourceSO, 
                PlayerTf = player
            });
            yield return new WaitForSeconds(_intervalBetweenDrop);
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (( _playerMask & (1 << other.gameObject.layer)) != 0)
        {
            StopCoroutine(_playerInTriggerCoroutine);
        }
    }
}