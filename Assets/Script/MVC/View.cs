using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//要显示，继承mono
public enum MViewName
{
    StartView,
    SelectView,
    MenuView,
    LoseView,
    CountDownView,
    SystemView,
    WinView,
    Spawner,
    PopupView,
}
public abstract class View : MonoBehaviour
{
    //标识其属于不同的视图
    public abstract MViewName Name { get; }

    //视图层需要关注的一个事件类型列表，由视图对象自己注册进去，因此要注册方法
    protected List<EventType> attentionEvents = new List<EventType>();

    //注册
    protected void RegisterEvent(EventType eventType)
    {
        if (ContainEventType(eventType))
        {
            return;
        }
        attentionEvents.Add(eventType);
    }
    //取消注册
    protected void UnregisterEvent(EventType eventType)
    {
        if (!ContainEventType(eventType))
        {
            return;
        }
        attentionEvents.Remove(eventType);
    }
    //取消注册所有
    protected void UnregisterAll()
    {
        attentionEvents.Clear();
    }
    //给调度中心查看是否有，如果有调度中心就执行操作
    public bool ContainEventType(EventType eventType)
    {
        return attentionEvents.Contains(eventType);
    }
    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventType">传入的事件类型</param>
    /// <param name="mEventArgs">事件执行函数</param>
    public abstract void HandleEvent(EventType eventType, MEventArgs mEventArgs);

    //去调度中心获取对象
    protected T GetModel<T>(MModelName name)
        where T:Model
    {
        return MVC.GetModel<T>(name);
    }

    protected T GetView<T>(MViewName name)
        where T : View
    {
        return MVC.GetView<T>(name);
    }

    protected void SendEvent(EventType eventType,MEventArgs args)
    {
        //mvc.sendevent;
        MVC.SendEvent(eventType, args);
    }

    protected virtual void Awake()
    {
        MVC.RegisterView(this);
        Initialize();
    }

    protected virtual void Start()
    {

    }
    protected virtual void OnDestroy()
    {
        MVC.UnRegisterView(this);
    }

    //初始化
    protected virtual void Initialize() { }

    public virtual void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
