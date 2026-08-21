using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//¼Ì³ÐmonoµÄµ¥Àý
public class MonoSingleton<T> : MonoBehaviour
    where T : MonoSingleton<T>
{
    protected MonoSingleton() { }
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("MonoSingleton");
            instance = go.AddComponent<T>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }
}
