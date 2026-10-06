using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfo
{
    public int ID;
    public string PrefabName;
    public float BaseSpeed;//基础速度
    public int BaseAttack;//基础攻击
    public bool IsLaser;//是否为激光类,是则会有延时删除
}
