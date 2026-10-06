using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseView : View
{
    public override MViewName Name => MViewName.LoseView;

    private int curRound;//当前波数
    private int totalRound;//总波数
    private int levelIdx;//当前关卡索引
    private Text txtCurrent;//设置当前波数
    private Text txtTotal;//设置当前总波数
    private Button btnRestart;//重开按钮
    
    public int CurRound
    {
        set
        {
            this.curRound = value;
            txtCurrent.text = curRound.ToString("D2");
        }
    }

    public int TotalRound
    {
        set
        {
            this.totalRound = value;
            txtTotal.text = totalRound.ToString();
        }
    }

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.StartLevel:
                //获取当前关卡索引
                MLevelArgs eLevelArgs = mEventArgs as MLevelArgs;
                eLevelArgs.LevelIndex = levelIdx;
                break;
            case EventType.Lose:
                //获取回合数据
                MRoundArgs rm = mEventArgs as MRoundArgs;
                this.CurRound = rm.CurRoundIndex;
                this.TotalRound = rm.TotalRound;
                break;
        }
    }

    protected override void Start()
    {
        base.Start();

        RegisterEvent(EventType.Lose);
        RegisterEvent(EventType.StartLevel);

        SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    protected override void Initialize()
    {
        base.Initialize();

        btnRestart = transform.Find("BtnRestart").GetComponent<Button>();
        txtCurrent = transform.Find("txtCurrent").GetComponent<Text>();
        txtTotal = transform.Find("txtTotal").GetComponent<Text>();

        btnRestart.onClick.AddListener(OnClickRestart);
    }

    private void OnClickRestart()
    {
        //重开
        Game.GetInstance().Pool.Clear();
        MLevelArgs args = new MLevelArgs(this.levelIdx, false);
        SendEvent(EventType.StartLevel, args);
    }
}
