using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Level level = new Level();
        Utils.LoadLevel("level0.xml", ref level);
        Debug.Log(Application.dataPath);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
