using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfo
{
    public int ID;
    public string PrefabName;
    public string NormalIcon;//可放置时图标
    public string DisabledIcon;//不可放置时图标
    public int MaxLevel;
    public int BasePrice;
    public float ShotRate;
    public float GuardRange;//索敌范围
    public int UseBulletID;
    public bool IsBottle;//是否为可转向的炮台
    public bool IsChain;//攻击是否连锁
}
