using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private string path;
    private Object prefab;
    private List<GameObject> list = new List<GameObject>();
    private List<GameObject> activeList = new List<GameObject>();
    public Pool(string path)
    {
        this.path = path;
        Load();
    }
    private void Load()
    {
        prefab = Resources.Load(this.path);
    }
    public GameObject Take()
    {
        GameObject go;
        if (list.Count <= 0)
        {
            go = GameObject.Instantiate(prefab) as GameObject;
        }
        else
        {
            go = list[0];
            list.RemoveAt(0);
        }
        activeList.Add(go);
        go.SetActive(true);
        IReusable iReusable = go.GetComponent<IReusable>();
        iReusable.Take();
        return go;
    }
    public void Back(GameObject obj)
    {
        if (!activeList.Contains(obj))
        {
            return;
        }
        else
        {
            list.Add(obj);
            activeList.Remove(obj);
            obj.SetActive(false);
        }
        IReusable reusable = obj.GetComponent<IReusable>();
        reusable.Back();

    }
    public void Clear()
    {
        if (list.Count > 0)
        {
            for(int i = 0; i < list.Count; i++)
            {
                GameObject.Destroy(list[i]);
            }
            list.Clear();
        }
        if (activeList.Count > 0)
        {
            for (int i = 0; i < activeList.Count; i++)
            {
                GameObject.Destroy(activeList[i]);
            }
            activeList.Clear();
        }
    }
    public bool Contain(GameObject go)
    {
        return activeList.Contains(go);
    }
}
