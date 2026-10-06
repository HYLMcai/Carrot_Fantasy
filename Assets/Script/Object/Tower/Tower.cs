using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour, IReusable
{
    public int level;//当前等级
    public int maxLevel;//最大等级
    private float shotRate;//开火间隔
    private float shotInterval;//
    private float timer;//
    Monster target;//锁定的敌人
    Monster targetChain;//连锁类炮塔锁定的下一个敌人
    Animator animator;//动画控件
    private float guardRange;//索敌范围

    public int Level
    {
        get => this.level;
        set
        {
            this.level = Mathf.Clamp(value, 0, maxLevel);
            transform.localScale = Vector3.one * (1 + level * 0.3f);
        }
    }

    public int MaxLevel { get; }
    public bool IsTopLevel { get => level >= maxLevel; }
    public float ShotTime { get => this.shotRate; }
    public float GuardRange { get => this.guardRange; }
    public int BasePrice { get; private set; }//自动属性
    public int Price { get => BasePrice * level; }
    public int SellPrice { get => (int)(Price * 0.8f); }
    public Tile Tile { get; private set; }
    public bool IsBottle { get; set; }//判断是否需要转动炮塔
    public bool IsChain { get; set; }//判断子弹是否连锁

    public void Back()
    {
        target = null;
        Tile = null;
        shotRate = 0;
        shotInterval = 0;
        level = 0;
        maxLevel = 0;
        BasePrice = 0;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            targetChain = null;
            Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            foreach(var monster in monsters)
            {
                if (!monster.IsDead && Vector3.Distance(transform.position, monster.Position) <= guardRange)
                {
                    target = monster;
                    break;
                }
            }
        }
        else
        {
            Monster[] monsters = GameObject.FindObjectsOfType<Monster>();
            if (targetChain == null)
            {
                foreach (var monster in monsters)
                {
                    if (!monster.IsDead && Vector3.Distance(transform.position, monster.Position) <= guardRange && monster != target)
                    {
                        targetChain = monster;
                        break;
                    }
                }
            }
            if (target.IsDead || Vector3.Distance(transform.position, target.Position) > GuardRange)
            {
                target = null;
                LookAt(null);
                return;
            }
            //炮塔转向判断
            if (IsBottle == true)
            {
                LookAt(target);
            }


            if (!IsChain)
            {
                //非激光类攻击
                //攻击间隔
                timer += Time.deltaTime;
                //攻击
                if (timer >= shotInterval)
                {
                    Shot(target);
                    timer = 0;
                }
            }
            else
            {
                Shot(target, targetChain);
            }
        }
    }

    void LookAt(Monster target)
    {
        if (target == null)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            //用弧度计算，不会出现360度的问题如90度=270度
            Vector3 dir = (target.Position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x);
            //弧度转化角度
            float eular = angle * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, eular - 90);
        }
    }

    public virtual void Shot(Monster target)
    {
        //子类实现
    }
    public virtual void Shot(Monster target,Monster targetChain)
    {
        //连锁攻击,子类实现
    }

    public void Load(Tile tile,TowerInfo info)
    {
        this.Tile = tile;
        //加载数据
        this.shotRate = info.ShotRate;
        this.shotInterval = 1 / this.shotRate;
        maxLevel = info.MaxLevel;
        Level = 1;
        BasePrice = info.BasePrice;
        guardRange = info.GuardRange;
        IsBottle = info.IsBottle;
        IsChain = info.IsChain;
    }

    public void Take()
    {
        //先找自己身上的，再往子对象去找
        animator = GetComponentInChildren<Animator>();
    }
}
