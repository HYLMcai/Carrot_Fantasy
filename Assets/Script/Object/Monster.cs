using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Monster0,
    Monster1,
    Monster2,
    Boss0,
    Boss1,
    Boss2,
}

public class Monster : Role
{
    //距离判定，判断是否碰到
    public const float CLOSE_DISTANCE = 0.1f;

    public event Action<Monster> ReachedEvent;

    int score = 0;//分数
    float moveSpeed;//移动速度
    Vector3[] path;//寻路路径
    int pointIdx = -1;//当前寻路点下标 
    bool isReached = false;//是否到达


    public float MoveSpeed
    {
        get { return this.moveSpeed; }
        set
        {
            this.moveSpeed = value;
        }
    }

    public bool IsReached
    {
        get { return this.isReached; }
    }
    
    public int Score
    {
        get { return this.score; }
    }

    public int Price { get; set; }//改

    public void Load(Vector3[] path,MonsterInfo monsterInfo)
    {
        this.path = path;
        
        this.moveSpeed = monsterInfo.MoveSpeed;
        this.score = monsterInfo.Price;
        this.MaxHp = monsterInfo.Hp;
        this.CurHp = this.MaxHp;
        this.Price = monsterInfo.Price;//改
        MoveNext();
    }

    bool HasNext()
    {
        //是否有下一个点
        return pointIdx < path.Length - 1;
    }

    void MoveNext()
    {
        if (!HasNext())
        {
            return;
        }
        //刚出来
        if (pointIdx == -1)
        {
            pointIdx = 0;
            MovePosition(path[0]);
        }
        else
        {
            pointIdx++;
        }
    }

    void MovePosition(Vector3 position)
    {
        this.transform.position = position;
    }

    private void Update()
    {
        //寻路
        if (IsReached) return;

        Vector3 pos = transform.position;
        Vector3 des = path[pointIdx];
        float dist = Vector3.Distance(pos, des);
        if (dist <= CLOSE_DISTANCE)
        {
            MovePosition(path[pointIdx]);
            if (HasNext())
            {
                MoveNext();
            }
            else
            {
                isReached = true;
                if (ReachedEvent != null)
                {
                    ReachedEvent(this);
                }
            }
        }
        else
        {
            Vector3 dir = des - pos;
            transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    public override void Take()
    {
        base.Take();
    }
    public override void Back()
    {
        base.Back();
        pointIdx = -1;
        moveSpeed = 0;
        MaxHp = 0;
        CurHp = 0;
        this.ReachedEvent = null;
        isReached = false;
        score = 0;
    }
}
