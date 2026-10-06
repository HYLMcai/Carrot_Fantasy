using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTower : Tower
{
    Transform shotPoint;
    GameObject mainLaser;//主射线
    GameObject chainLaser;//副射线
    Monster tempTarget;
    Monster tempLaserTarget;
    bool IsFire { get; set; }

    private void Awake()
    {
        shotPoint = transform.Find("LightningBody/ShotPoint");
    }
    public override void Shot(Monster target,Monster targetChain)
    {
        base.Shot(target, targetChain);
        if (tempTarget != target)
        {
            tempTarget = target;
            IsFire = false;
            
        }
        if (IsFire) return;
        //主射线初始化
        mainLaser = Game.GetInstance().Pool.Take("Bullet/LightningBullet");
        LightningBullet mainBullet = mainLaser.GetComponent<LightningBullet>();
        mainLaser.transform.position = shotPoint.position;

        //生成主射线
        mainBullet.Load(1, target, GuardRange);

        //副射线初始化
        chainLaser = Game.GetInstance().Pool.Take("Bullet/LightningBullet");
        LightningBullet chainBullet = chainLaser.GetComponent<LightningBullet>();
        chainLaser.transform.position = target.Position;
        //生成副射线
        chainBullet.Load(1, targetChain, mainLaser);

        IsFire = true;
    }
    protected override void Update()
    {
        base.Update();
        if (mainLaser != null)
        {
            mainLaser.transform.position = shotPoint.position;
            chainLaser.transform.position = tempTarget.Position;
        }
    }
}
