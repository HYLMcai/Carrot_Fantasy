using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//速度枚举
public enum GameSpeed
{
    One=1,
    Two=2,
}

public class MenuView : View
{
    public override MViewName Name => MViewName.MenuView;

    private int score;
    private int curRound;
    private int totalRound;
    private GameSpeed gameSpeed;
    private bool isPlaying = false;

    private Text txtScore;
    private Text txtCurRound;
    private Text txtTotalRound;
    private GameObject pauseImage;
    private GameObject btnSpeedOne;
    private GameObject btnSpeedTwo;
    private GameObject btnResume;
    private GameObject btnPause;
    private GameObject RoundInfo;

    //给UI上添加数据
    public int Score
    {
        get { return this.score; }
        set
        {
            score = Mathf.Clamp(value, 0, int.MaxValue);
            txtScore.text = score.ToString();
        }
    }
    public int CurRound
    {
        get { return this.curRound; }
        set
        {
            this.curRound = Mathf.Clamp(value, 0, int.MaxValue);
            txtCurRound.text = curRound.ToString("D2");
        }
    }
    public int TotalRound
    {
        get { return this.totalRound; }
        set
        {
            this.totalRound = Mathf.Clamp(value, 0, int.MaxValue);
            txtTotalRound.text = curRound.ToString("D2");
        }
    }
    public GameSpeed GamePlaySpeed
    {
        get { return this.gameSpeed; }
        set
        {
            this.gameSpeed = value;
            this.btnSpeedOne.SetActive(this.gameSpeed == GameSpeed.One);
            this.btnSpeedTwo.SetActive(this.gameSpeed == GameSpeed.Two);
        }
    }
    public bool IsPlaying
    {
        get { return this.isPlaying; }
        set
        {
            this.isPlaying = value;
            this.btnResume.SetActive(!this.isPlaying);
            this.pauseImage.SetActive(!this.isPlaying);
            this.btnPause.SetActive(this.isPlaying);
            this.RoundInfo.SetActive(this.isPlaying);
        }
    }

    protected override void Start()
    {
        base.Start();
        RegisterEvent(EventType.StartRound);

        this.Score = 0;
        this.IsPlaying = true;
        this.GamePlaySpeed = GameSpeed.One;
        this.CurRound = 0;
        this.totalRound = 0;
    }

    protected override void Initialize()
    {
        base.Initialize();

        RoundInfo = transform.Find("RoundInfo").gameObject;

        txtScore = transform.Find("Score").GetComponent<Text>();
        Transform tfRoundInfo = transform.Find("RoundInfo");
        txtCurRound = tfRoundInfo.Find("txtCurrent").GetComponent<Text>();
        txtTotalRound = tfRoundInfo.Find("txtTotal").GetComponent<Text>();

        btnSpeedOne = transform.Find("BtnSpeed1").gameObject;
        btnSpeedOne.GetComponent<Button>().onClick.AddListener(OnSpeedOneClick);

        btnSpeedTwo = transform.Find("BtnSpeed2").gameObject;
        btnSpeedTwo.GetComponent<Button>().onClick.AddListener(OnSpeedTwoClick);

        btnResume = transform.Find("BtnResume").gameObject;
        btnResume.GetComponent<Button>().onClick.AddListener(OnResumeClick);

        btnPause = transform.Find("BtnPause").gameObject;
        btnPause.GetComponent<Button>().onClick.AddListener(OnPauseClick);

        transform.Find("BtnSystem").GetComponent<Button>().onClick.AddListener(OnSystemClick);

        pauseImage = transform.Find("PauseImage").GetComponent<Image>().gameObject;
    }

    private void OnSpeedOneClick()
    {
        this.GamePlaySpeed = GameSpeed.Two;
        Time.timeScale = 2;
    }

    private void OnSpeedTwoClick()
    {
        this.GamePlaySpeed = GameSpeed.One;
        Time.timeScale = 1;
    }

    private void OnResumeClick()
    {
        this.IsPlaying = true;
        Time.timeScale = 1;
    }

    private void OnPauseClick()
    {
        this.IsPlaying = false;
        Time.timeScale = 0;
    }

    private void OnSystemClick()
    {
        View systemView = GetView<SystemView>(MViewName.SystemView);
        systemView.SetActive(true);
        if (!IsPlaying)
        {
            Time.timeScale = 0;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnregisterAll();
    }

    public override void HandleEvent(EventType eventType, MEventArgs mEventArgs)
    {
        switch (eventType)
        {
            case EventType.StartRound:
                MRoundArgs e = mEventArgs as MRoundArgs;
                OnRoundInfoUpdate(e.CurRoundIndex, e.TotalRound);
                this.Score = GetModel<GameModel>(MModelName.GameModel).Gold;
                break;
        }
    }
    void OnRoundInfoUpdate(int curRound,int totalRound)
    {
        this.CurRound = curRound + 1;
        this.TotalRound = totalRound;
    }
}