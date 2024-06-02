using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourcePooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ResourceSO Resource;
        public int Size;
    }

    public static ResourcePooler Instance;

    [SerializeField] private List<Pool> _pools;
    private Dictionary<ResourceSO, Queue<Resource>> _poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _poolDictionary = new Dictionary<ResourceSO, Queue<Resource>>();

        foreach (Pool pool in _pools)
        {
            Queue<Resource> objectPool = new Queue<Resource>();

            for (int i = 0; i < pool.Size; i++)
            {
                Resource res = Instantiate(pool.Resource.Prefab).GetComponent<Resource>();
                res.gameObject.SetActive(false);
                objectPool.Enqueue(res);
            }

            _poolDictionary.Add(pool.Resource, objectPool);
        }
    }

    public Resource SpawnFromPool(ResourceSO resourceSO, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(resourceSO))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Resource resourceToSpawn = _poolDictionary[resourceSO].Dequeue();

        resourceToSpawn.gameObject.SetActive(true);
        resourceToSpawn.transform.position = position;
        resourceToSpawn.transform.rotation = rotation;
        
        _poolDictionary[resourceSO].Enqueue(resourceToSpawn);

        return resourceToSpawn;
    }
}
