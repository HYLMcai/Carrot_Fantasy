using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IReusable
{
    private int Level { get; set; }//等级
    public float BaseSpeed { get; private set; }//基础弹速
    public int BaseAttack { get; private set; }//基础攻击力
    public float MoveSpeed { get => BaseSpeed * Level; }//实际弹速
    public float Attack { get => BaseAttack * Level; }//实际攻击力
    public float DelayTime = 1f;//延迟回收
    protected bool IsExploded = false;//是否爆炸
    public bool IsLaser { get;private set; }//是否为激光

    protected virtual void Awake()
    {
        
    }

    protected virtual void Update()
    {

    }

    public void Load(int level,int bulletID)
    {
        BulletInfo info = Game.GetInstance().StaticData.GetBulletInfo(bulletID);

        this.Level = level;
        this.BaseSpeed = info.BaseSpeed;
        this.BaseAttack = info.BaseAttack;
        this.IsLaser = info.IsLaser;
        if (!IsLaser) StartCoroutine("DelayDestroy");
    }

    public virtual void Exploded()
    {
        if (IsExploded) return;
        IsExploded = true;
        //销毁子弹
        Game.GetInstance().Pool.Back(this.gameObject);
        if (!IsLaser) StopCoroutine("DelayDestroy");
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(DelayTime);
        Exploded();
    }

    public virtual void Back()
    {
        IsExploded = true;
        Level = 0;
        BaseSpeed = 0;
        BaseAttack = 0;
    }

    public virtual void Take()
    {
        IsExploded = false;
    }
}
