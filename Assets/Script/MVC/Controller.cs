using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//位于模型层与视图层之间，不需要表现出来
public abstract class Controller
{
    //获取二者的调度中心
    public T GetModel<T>(MModelName name) where T : Model
    {
        return MVC.GetModel<T>(name);
    }
    public T GetView<T>(MViewName name) where T : View
    {
        return MVC.GetView<T>(name); 
    }

    //注册
    public void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }
    public void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }
    public void RegisterController(EventType eventType,Type tType)
    {
        MVC.RegisterController(eventType, tType);
    }

    //执行函数,每个控制层执行的内容不一样，因此是抽象类
    public abstract void Excute(MEventArgs args);
}
