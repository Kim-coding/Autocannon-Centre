using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance {  get; private set; }

    private Dictionary<string, List<GameObject>> poolDictionary = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        instance = this;
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        string poolKey = prefab.name;
        List<GameObject> objectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++) 
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Add(obj);
        }

        poolDictionary.Add(poolKey, objectPool);
    }

    public GameObject GetObjectPool(string poolKey) 
    {
        if(!poolDictionary.ContainsKey(poolKey))
        {
            return null;
        }

        List<GameObject> objectPool = poolDictionary[poolKey];
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(objectPool[0]);
        newObj.SetActive(true);
        objectPool.Add(newObj);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
