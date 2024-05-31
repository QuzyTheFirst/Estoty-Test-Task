using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ResourceType resourceType;
        public GameObject prefab;
        public int size;
    }
    
    public enum ResourceType
    {
        Wood,
        Rock,
        Metal
    }

    public static ObjectPooler Instance;

    [SerializeField] private List<Pool> _pools;
    private Dictionary<ResourceType, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _poolDictionary = new Dictionary<ResourceType, Queue<GameObject>>();

        foreach (Pool pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.resourceType, objectPool);
        }
    }

    public GameObject SpawnFromPool(ResourceType resourceType, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(resourceType))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = _poolDictionary[resourceType].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        _poolDictionary[resourceType].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
