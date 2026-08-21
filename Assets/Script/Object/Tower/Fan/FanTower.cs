using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTower : Tower
{
    Transform shotPoint;

    private void Awake()
    {
        shotPoint = transform.Find("Fan/ShotPoint");
    }

    public override void Shot(Monster target)
    {
        base.Shot(target);
        GameObject go = Game.GetInstance().Pool.Take("Bullet/FanBullet");
        FanBullet bullet = go.GetComponent<FanBullet>();
        go.transform.position = shotPoint.position;
        bullet.Load(1, target);
    }
}
