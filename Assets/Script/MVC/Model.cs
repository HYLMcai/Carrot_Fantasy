using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MModelName
{
    GameModel,
    RoundModel,
}

//不用继承mono，管理数据和逻辑的地方，不需要有具体表现
public abstract class Model
{
    //标识其属于不同的模型
    public abstract MModelName Name { get; }

    public void SendEvent(EventType eventType,MEventArgs mEventArgs)
    //MEventArgs mEventArgs可换成params object[] p用可变参数封拆箱的方法。
    //MEventArgs mEventArgs这个用起来很麻烦，因为每一个事件都要定义一个类作为载体，多了就很麻烦，但是从性能上讲这个优于可变参数封拆箱
    {
        //MVC调度中心
        MVC.SendEvent(eventType, mEventArgs);
    }
}
