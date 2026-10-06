using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBullet : Bullet
{
    //获取敌人，使子弹朝敌人运动
    public Monster Target { get; private set; }
    public Vector3 Direction { get; private set; }
    public float GuardRange { get; private set; }
    private bool IsMain { get; set; }
    private GameObject MainLaser { get; set; }//副射线获取主射线
    float atk = 0;


    public void Load(int level, Monster monster, float guardRange)
    {
        Load(level, 3);
        Target = monster;
        GuardRange = guardRange;
        IsMain = true;

    }
    public void Load(int level, Monster monster, GameObject mainLaser)
    {
        Load(level, 3);
        Target = monster;
        MainLaser = mainLaser;
        IsMain = false;
    }
    protected override void Update()
    {
        base.Update();
        if (IsExploded) return;

        if (Target != null)
        {
            if (Target.IsDead)
            {
                Exploded();
                return;
            }
            Direction = (Target.Position - transform.position).normalized;
            float angle = Mathf.Atan2(Direction.y, Direction.x);
            //弧度转化角度,实现子弹跟随目标
            float eular = angle * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, eular - 90);
            //计算敌人与炮塔的距离
            float distance = Mathf.Sqrt(Mathf.Pow(Target.Position.x - transform.position.x, 2) + Mathf.Pow(Target.Position.y - transform.position.y, 2));
            transform.localScale = new Vector3(1, distance * 0.5f, 1);

            //激光武器不用检测打没打到,打中不销毁，持续输出直至低人死亡
            atk += Time.deltaTime;
            if (atk >= 0.1)
            {
                Target.TakeDamge((int)this.Attack);
                atk = 0;
            }

            //敌人离开攻击范围时自动删除(主射线)
            if (distance >= GuardRange && IsMain == true)
            {
                Exploded();
            }
            //主射线销毁后(换目标),跟随主射线一起销毁
            //主副射线判断在前，否则会因为主射线没有传入主射线导致报错(不影响使用，但不好看)
            if (IsMain == false && !MainLaser.activeSelf)
            {
                Exploded();
            }

        }
        else
        {
            Exploded();
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
