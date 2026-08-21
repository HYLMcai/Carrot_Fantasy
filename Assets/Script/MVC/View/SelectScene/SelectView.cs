using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SelectView : View
{
    private List<Level> levels;
    private int curIdx = 0;
    private int selectIdx = -1;

    private List<LevelCard> cards = new List<LevelCard>();
    private GameObject leftBtn;
    private GameObject rightBtn;
    private GameObject startLevelBtn;
    private GameObject clearPrefsBtn;
    private GameObject ExitBtn;
    private GameObject lockObj;
    public override MViewName Name => MViewName.SelectView;

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        
    }
    protected override void Start()
    {
        base.Start();
        //1.读取并加载关卡列表2.读取存档数据
        LoadLevels();
        //3.刷新界面
        SetSelectIdx(0);
    }

    private void SetSelectIdx(int selectIdx)
    {
        if (selectIdx != this.selectIdx && selectIdx < levels.Count)
        {
            this.selectIdx = selectIdx;
            RefreshView();
        }
    }

    private void RefreshView()
    {
        int leftIdx = this.selectIdx - 1;
        int rightIdx = this.selectIdx + 1;
        //刷新卡
        if (this.selectIdx < levels.Count)
        {
            cards[0].SetLevelInfo(this.selectIdx == 0 ? null : levels[leftIdx]);
            cards[0].SetMask(leftIdx > this.curIdx);
            cards[1].SetLevelInfo(levels[this.selectIdx]);
            cards[1].SetMask(this.selectIdx > this.curIdx);
            cards[2].SetLevelInfo(this.selectIdx == (levels.Count - 1) ? null : levels[rightIdx]);
            cards[2].SetMask(rightIdx > this.curIdx);
        }

        //刷新按钮
        leftBtn.SetActive(this.selectIdx > 0);
        rightBtn.SetActive(this.selectIdx < (this.levels.Count - 1));

        startLevelBtn.SetActive(this.selectIdx <= this.curIdx);
        lockObj.SetActive(this.selectIdx > this.curIdx);
    }

    private void LoadLevels()
    {
        GameModel model = GetModel<GameModel>(MModelName.GameModel);
        levels = model.Levels;
        curIdx = model.CurLeveldx;
    }

    protected override void Initialize()
    {
        base.Initialize(); 
         Transform levelCard = transform.Find("LevelCard");
        cards.Add(new LevelCard(levelCard.Find("LevelImage_Left").gameObject));
        cards.Add(new LevelCard(levelCard.Find("LevelImage_Middle").gameObject));
        cards.Add(new LevelCard(levelCard.Find("LevelImage_Right").gameObject));

        leftBtn = transform.Find("LeftBtn").gameObject;
        rightBtn = transform.Find("RightBtn").gameObject;
        leftBtn.GetComponent<Button>().onClick.AddListener(OnLeftBtnClick);
        rightBtn.GetComponent<Button>().onClick.AddListener(OnRightBtnClick);

        startLevelBtn = transform.Find("StartLevelBtn").gameObject;
        lockObj = transform.Find("LockLevel").gameObject;
        startLevelBtn.GetComponent<Button>().onClick.AddListener(OnStartClick);

        clearPrefsBtn = transform.Find("ClearLevelData").gameObject;
        clearPrefsBtn.GetComponent<Button>().onClick.AddListener(OnClearClick);

        ExitBtn = transform.Find("ExitBtn").gameObject;
        ExitBtn.GetComponent<Button>().onClick.AddListener(OnExitClick);
    }

    private void OnRightBtnClick()
    {
        int selectIdx = Mathf.Clamp((this.selectIdx + 1), 0, this.levels.Count - 1);
        SetSelectIdx(selectIdx);
    }
    private void OnLeftBtnClick()
    {
        int selectIdx = Mathf.Clamp((this.selectIdx - 1), 0, this.levels.Count - 1);
        SetSelectIdx(selectIdx);
    }
    private void OnStartClick()
    {
        MLevelArgs args = new MLevelArgs(this.selectIdx, false);
        SendEvent(EventType.StartLevel, args);
    }
    private void OnClearClick()
    {
        GameModel gm = GetModel<GameModel>(MModelName.GameModel);
        gm.ClearGameProgress();
        Game.GetInstance().LoadScene(1);
    }

    private void OnExitClick()
    {
        Game.GetInstance().LoadScene(1);
    }
}
