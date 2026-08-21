using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShitTower : Tower
{
    Transform shotPoint;

    private void Awake()
    {
        shotPoint = transform.Find("ShitBody/ShotPoint");
    }
    public override void Shot(Monster target)
    {
        base.Shot(target);
        GameObject go = Game.GetInstance().Pool.Take("Bullet/ShitBullet");
        ShitBullet bullet = go.GetComponent<ShitBullet>();
        go.transform.position = shotPoint.position;
        bullet.Load(1, target);
    }
}
