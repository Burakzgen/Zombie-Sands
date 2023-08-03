using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool shouldExpand;
        public GameObject parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = pool.parent.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Queue<GameObject> objectsInPool = poolDictionary[tag];
        GameObject objectToSpawn = null;

        foreach (var obj in objectsInPool)
        {
            if (!obj.activeInHierarchy)
            {
                objectToSpawn = obj;
                break;
            }
        }

        if (objectToSpawn == null && pools.Find(pool => pool.tag == tag).shouldExpand)
        {
            GameObject obj = Instantiate(pools.Find(pool => pool.tag == tag).prefab);
            obj.transform.parent = pools.Find(pool => pool.tag == tag).parent.transform;
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
            objectToSpawn = obj;
        }

        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }
 

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}
/*
 *    public GameObject SpawnFromPool(string tag, Transform transform)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        if (poolDictionary[tag].Count == 0 && pools.Find(pool => pool.tag == tag).shouldExpand)
        {
            GameObject obj = Instantiate(pools.Find(pool => pool.tag == tag).prefab);
            obj.transform.parent = pools.Find(pool => pool.tag == tag).parent.transform;
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(transform.position, transform.rotation);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
 */