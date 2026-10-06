using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBullet : Bullet
{
    //获取敌人，使子弹朝敌人运动
    public Monster Target { get; private set; }
    public Vector3 Direction { get; private set; }

    public void Load(int level,Monster monster)
    {
        Load(level, 0);
        Target = monster;

        Direction = (Target.Position - transform.position).normalized;
    }
    protected override void Update()
    {
        base.Update();
        if (IsExploded) return;

        if (Target != null)
        {
            if (Target.IsDead)
            {
                //算目标方向
                Direction = (Target.Position - transform.position).normalized;
            }
            transform.Translate(Direction * MoveSpeed * Time.deltaTime);
            
            //如果子弹靠近敌人到一定距离则销毁，敌人受伤
            if (Vector3.Distance(transform.position, Target.Position) <= Monster.CLOSE_DISTANCE)
            {
                Target.TakeDamge((int)this.Attack);
                Exploded();
            }
        }
        else
        {
            transform.Translate(Direction * MoveSpeed * Time.deltaTime);
        }
    }
    public override void Back()
    {
        base.Back();
        Target = null;
    }
    public override void Take()
    {
        base.Take();
    }
}
