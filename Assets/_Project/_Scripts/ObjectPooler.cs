using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ResourceSO Resource;
        public int Size;
    }

    public static ObjectPooler Instance;

    [SerializeField] private List<Pool> _pools;
    private Dictionary<ResourceSO, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _poolDictionary = new Dictionary<ResourceSO, Queue<GameObject>>();

        foreach (Pool pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Resource.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Resource, objectPool);
        }
    }

    public GameObject SpawnFromPool(ResourceSO resourceSO, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(resourceSO))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[resourceSO].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        _poolDictionary[resourceSO].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
