using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemView : View
{
    public override MViewName Name => MViewName.SystemView;

    private int levelIdx;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.StartLevel:
                //获取当前关卡索引
                MLevelArgs eLevelArgs = mEventArgs as MLevelArgs;
                eLevelArgs.LevelIndex = levelIdx;
                break;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        transform.Find("BtnContinue").GetComponent<Button>().onClick.AddListener(OnContinue);
        transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(OnRestart);
        transform.Find("BtnSelect").GetComponent<Button>().onClick.AddListener(OnSelect);
        transform.Find("BtnClose").GetComponent<Button>().onClick.AddListener(OnContinue);
    }

    protected override void Start()
    {
        base.Start();
        SetActive(false);
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
        MenuView view = GetView<MenuView>(MViewName.MenuView);
        if (view != null)
        {
            GetView<MenuView>(MViewName.MenuView).IsPlaying = !active;
            Time.timeScale = 1;
        }
    }
    //继续按钮
    private void OnContinue()
    {
        SetActive(false);
    }
    //选关按钮
    private void OnSelect()
    {
        Game.GetInstance().LoadScene(2);
        MLevelArgs args = new MLevelArgs(this.levelIdx);
        SendEvent(EventType.EndLevel, args);
    }
    //重开按钮
    private void OnRestart()
    {
        Game.GetInstance().Pool.Clear();
        MLevelArgs args = new MLevelArgs(this.levelIdx);
        SendEvent(EventType.Restart, null);
        SendEvent(EventType.StartLevel, args);
    }
}
