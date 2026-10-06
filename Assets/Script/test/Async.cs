using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Async : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //异步加载
        for (int i = 0; i < 1000; i++)
        {
            //读取的是resource里面的文件，不用带resource
            ResourceRequest request = Resources.LoadAsync<UnityEngine.Object>("Prefab/Cube");
            request.completed += Load;
        }
    }

    void Load(AsyncOperation request)
    {
        ResourceRequest prefab = request as ResourceRequest;
        UnityEngine.Object uo = prefab.asset;
        GameObject go = GameObject.Instantiate(uo) as GameObject;
        float v1 = UnityEngine.Random.Range(0, 10);
        float v2 = UnityEngine.Random.Range(0, 10);
        float v3 = UnityEngine.Random.Range(0, 10);
        go.transform.position = new Vector3(v1, v2, v3);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
