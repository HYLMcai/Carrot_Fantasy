using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private string resourcePath = "Prefab";
    private Dictionary<string, Pool> poolMap = new Dictionary<string, Pool>();

    public GameObject Take(string path)
    {
        if (!string.IsNullOrEmpty(resourcePath))
        {
            path = resourcePath + "/" + path;
        }
        if (!poolMap.ContainsKey(path))
        {
            RegisterPool(path);
        }
        return poolMap[path].Take();
    }
    void RegisterPool(string path)
    {
        Pool pool = new Pool(path);
        poolMap.Add(path, pool);
    }
    public void Back(GameObject obj)
    {
        foreach(var pool in poolMap.Values)
        {
            if (pool.Contain(obj))
            {
                pool.Back(obj);
                return;
            }
        }
    }
    public void Clear()
    {
        foreach(var pool in poolMap.Values)
        {
            pool.Clear();
        }
        poolMap.Clear();
    }

}
