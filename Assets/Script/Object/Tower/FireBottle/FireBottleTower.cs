using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBottleTower : Tower
{
    Transform shotPoint;
    GameObject go;
    Monster tempTarget;
    bool IsFire { get; set; }

    private void Awake()
    {
        shotPoint = transform.Find("FireBottleBody/ShotPoint");
    }
    public override void Shot(Monster target)
    {
        if (tempTarget != target)
        {
            tempTarget = target;
            IsFire = false;
        }
        if (IsFire) return;
        base.Shot(target);
        go = Game.GetInstance().Pool.Take("Bullet/FireBullet");
        FireBullet bullet = go.GetComponent<FireBullet>();
        go.transform.position = shotPoint.position;
        bullet.Load(1, target, GuardRange);
        IsFire = true;
    }
    protected override void Update()
    {
        base.Update();
        if (go != null)
        {
            go.transform.position = shotPoint.position;
        }
    }
}
