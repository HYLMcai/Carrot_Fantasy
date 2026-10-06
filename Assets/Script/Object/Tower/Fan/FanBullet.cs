using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBullet : Bullet
{
    //获取敌人，使子弹朝敌人运动
    public Monster Target { get; private set; }
    public Vector3 Direction { get; private set; }

    public void Load(int level, Monster monster)
    {
        Load(level, 1);
        Target = monster;

        Direction = (Target.Position - transform.position).normalized;
    }
    protected override void Update()
    {
        base.Update();
        if (IsExploded) return;

        if (Target != null)
        {
            //子弹不需要算目标方向角度，往一个方向飞就完了
            transform.Translate(Direction * MoveSpeed * Time.deltaTime);

            //穿透子弹不销毁，等自动回收
            if (Vector3.Distance(transform.position, Target.Position) <= Monster.CLOSE_DISTANCE)
            {
                Target.TakeDamge((int)this.Attack);
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
