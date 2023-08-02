using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    private GameObject _objectPoolEmptyHolder;

    private static GameObject _particleSystemEmpty;
    private static GameObject _gameObjectsEmpty;
    private static GameObject _enemyObjectsEmpty;
    public enum PoolType
    {
        ParticalSystem,
        GameObject,
        Enemy,
        None
    }
    public static PoolType PoolingType;
    private void Awake()
    {
        SetupEmpties();
    }
    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemEmpty = new GameObject("Particle Effects");
        _particleSystemEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("GameObjects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _enemyObjectsEmpty = new GameObject("Enemy Characters");
        _enemyObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }
    public static GameObject SpawnObject(GameObject objectToSpawm, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawm.name);
             
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawm.name };
            ObjectPools.Add(pool);
        }


        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();
    
        if (spawnableObj == null)
        {
            GameObject parentObject = SetParentObject(poolType);

            spawnableObj = Instantiate(objectToSpawm, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }
    public static GameObject SpawnObject(GameObject objectToSpawm,Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawm.name);
          

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawm.name };
            ObjectPools.Add(pool);
        }


        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();
       
        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawm, parentTransform);

        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if (pool == null)
        {
            Debug.LogError("Trying to release an object that is not pooled" + obj.name);
        }
        else
        {

            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticalSystem:
                return _particleSystemEmpty;
            case PoolType.GameObject:
                return _gameObjectsEmpty;
            case PoolType.Enemy:
                return _enemyObjectsEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}
public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
