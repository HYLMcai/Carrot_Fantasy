using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class WinView : View
{
    public override MViewName Name => MViewName.WinView;

    private int curRound;//当前回合数
    private int totalRound;//总回合数
    private int levelIdx;//当前关卡索引
    private Text txtCurrent;//显示当前回合数
    private Text txtTotal;//显示总回合数
    private Button btnContinue;//继续游戏按钮
    private Button btnRestart;//重开按钮

    //设置当前回合数的显示
    public int CurRound 
    { 
        set
        {
            curRound = value;
            txtCurrent.text = (curRound + 1).ToString("D2");
        }
    }
    //设置当前总回合数显示
    public int TotalRound
    {
        set
        {
            totalRound = value;
            txtTotal.text = totalRound.ToString();
        }
    }

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.StartLevel:
                //获取关卡索引
                MLevelArgs eLevel = mEventArgs as MLevelArgs;
                this.levelIdx = eLevel.LevelIndex;
                break;
            case EventType.Win:
                //从回合管理里拿数据
                MRoundArgs eRound = mEventArgs as MRoundArgs;
                this.CurRound = eRound.CurRoundIndex;
                this.TotalRound = eRound.TotalRound;
                break;
        }
    }

    protected override void Start()
    {
        base.Start();

        RegisterEvent(EventType.StartLevel);
        RegisterEvent(EventType.Win);
        
        SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    protected override void Initialize()
    {
        txtCurrent = transform.Find("txtCurrent").GetComponent<Text>();
        txtTotal = transform.Find("txtTotal").GetComponent<Text>();
        btnContinue = transform.Find("BtnContinue").GetComponent<Button>();
        btnRestart = transform.Find("BtnRestart").GetComponent<Button>();

        btnContinue.onClick.AddListener(OnClickContinue);
        btnRestart.onClick.AddListener(OnClickRestart);

        base.Initialize();
    }

    private void OnClickContinue()
    {
        Game.GetInstance().LoadScene(2);
        Game.GetInstance().Pool.Clear();
    }

    private void OnClickRestart()
    {
        //重开
        Game.GetInstance().Pool.Clear();
        MLevelArgs args = new MLevelArgs(this.levelIdx, true);
        SendEvent(EventType.StartLevel, args);
    }
}
