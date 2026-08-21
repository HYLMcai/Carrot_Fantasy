using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleTower : Tower
{
    Transform shotPoint;

    private void Awake()
    {
        shotPoint = transform.Find("BottleBody/ShotPoint");
    }
    public override void Shot(Monster target)
    {
        base.Shot(target);
        GameObject go = Game.GetInstance().Pool.Take("Bullet/BottleBullet");
        BottleBullet bullet = go.GetComponent<BottleBullet>();
        go.transform.position = shotPoint.position;
        bullet.Load(1, target);
    }
}
