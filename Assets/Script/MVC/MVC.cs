using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MVC
{
    //将三个注册进来
    private static Dictionary<MModelName, Model> models = new Dictionary<MModelName, Model>();
    private static Dictionary<MViewName, View> views = new Dictionary<MViewName, View>();
    private static Dictionary<EventType, Type> commandMap = new Dictionary<EventType, Type>();
    /// <summary>
    /// 注册Model层
    /// </summary>
    /// <param name="model">model层对象</param>
    public static void RegisterModel(Model model)
    {
        if (models.ContainsKey(model.Name))
        {
            Debug.LogError("模型层重复注册" + model.Name);
            return;
        }
        models.Add(model.Name, model);
    }
    /// <summary>
    /// 注册视图层
    /// </summary>
    /// <param name="view">视图层对象</param>
    public static void RegisterView(View view)
    {
        if (views.ContainsKey(view.Name))
        {
            Debug.LogError("视图层重复注册" + view.Name);
            return;
        }
        views.Add(view.Name, view);
        
    }
    public static void UnRegisterView(View view)
    {
        if (!views.ContainsKey(view.Name))
        {
            Debug.LogError("视图层不存在，不能移除" + view.Name);
            return;
        }
        views.Remove(view.Name);
    }

    /// <summary>
    /// 注册控制层
    /// </summary>
    /// <param name="eventType">请求类型</param>
    /// <param name="controllerType">处理请求对应的控制器（脚本文件）</param>
    public static void RegisterController(EventType eventType,Type controllerType)
    {
        if (commandMap.ContainsKey(eventType))
        {
            Debug.LogError("控制器重复注册" + eventType);
            return;
        }
        commandMap.Add(eventType, controllerType);
    }
    public static T GetModel<T>(MModelName name)
        where T:Model
    {
        Model model = null;
        models.TryGetValue(name, out model);
        return model as T;
    }
    public static T GetView<T>(MViewName name) 
        where T : View
    {
        View view = null;
        views.TryGetValue(name, out view);
        return view as T;
    }
    public static void SendEvent(EventType eventType,MEventArgs mEventArgs)
    {
        //控制层出来协调
        Type tType = null;//因为反射CreateInstance接收的是type类型因此定义一个type类型参数接收
        //判断eventType是否存在于commandMap中，存在则传入tType中，且该方法能将返回值自动转化为bool类型
        if (commandMap.TryGetValue(eventType, out tType))
        {
            Controller controller = Activator.CreateInstance(tType) as Controller;
            controller.Excute(mEventArgs);
        }
        //视图层
        foreach(View view in views.Values)
        {
            if (view.ContainEventType(eventType))
            {
                view.HandleEvent(eventType, mEventArgs);
            }
        }
    }
}
