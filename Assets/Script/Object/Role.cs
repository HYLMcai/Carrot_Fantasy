using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role : MonoBehaviour, IReusable
{
    //做一个委托，当发生时调用该事件则可以知道事件发生
    public event Action<int, int> HpEvent; 
    public event Action<Role> DeadEvent; 
    int curHp;
    int maxHp;

    public int CurHp
    {
        get { return this.curHp; }
        set
        {
            value = Mathf.Clamp(value, 0, maxHp);
            //刷新
            if (value == this.curHp)
            {
                //防止无用执行
                return;
            }
            //存值
            this.curHp = value;
            if (HpEvent != null)
            {
                HpEvent(this.curHp, this.maxHp);
            }

            if (this.curHp <= 0)
            {
                if (DeadEvent != null)
                {
                    DeadEvent(this);
                }
            }
        }
    }
    public int MaxHp
    {
        get { return this.maxHp; }
        set
        {
            value = Mathf.Clamp(value, 0, int.MaxValue);
            if (value == this.maxHp)
            {
                //防止无用执行
                return;
            }
            //存值
            this.maxHp = value;
            if (HpEvent != null)
            {
                HpEvent(this.curHp, this.maxHp);
            }
        }
    }

    //死亡判断
    public bool IsDead { get { return this.curHp <= 0; } }

    //敌人位置（用于炮塔）
    public Vector3 Position
    {
        get => this.transform.position;
        set => this.transform.position = value;
    }

    //扣血处理
    public virtual void TakeDamge(int hit)
    {
        if (IsDead)
        {
            return;
        }
        this.CurHp -= hit;
    }

    //死亡事件
    public virtual void OnDead(Role role)
    {
        //根据不同的子类重写
    }
    
    public virtual void Back()
    {
        DeadEvent = null;
        HpEvent = null;

        curHp = 0;
        maxHp = 0;
    }

    public virtual void Take()
    {
        DeadEvent += this.OnDead;
    }
}
