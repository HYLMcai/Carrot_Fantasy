using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//不继承mono的单例
public abstract class Singleton<T> where T: Singleton<T>
{
    protected Singleton() { }
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            Type p = typeof(T);
            //用反射创建实例
            instance = Activator.CreateInstance(p) as T;
            instance.Initial();
        }
        return instance;
    }
    protected abstract void Initial();
}
